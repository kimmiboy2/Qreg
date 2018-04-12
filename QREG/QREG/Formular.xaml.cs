using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Flurl.Http;
using Flurl.Util;
using Newtonsoft.Json.Linq;
using QREG.DynamicUI;
using QREG.Utilities;

namespace QREG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Formular : CoolContentPage
    {
        JSONFetcher jsonFetcher = new JSONFetcher(null);
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        FlurlClient flurlClient = FlurlClient_Singleton.GetInstance();
        List<AbstractDynamicUI> dynamicUIList = new List<AbstractDynamicUI>();
        string overskrift, afvigelse, korrigerendehandling, forslag, handleplan, kategori, url;
        string templateArrayNumber;
        string templateTitle;
        string formtemplateid;
        string templateversion;

        List<Element> elementsList = new List<Element>();

        public Formular(string templateArrayNumber)
        {
            InitializeComponent();

            if (EnableBackButtonOverride)
            {
                CustomBackButtonAction = async () =>
                {
                    bool result = await hasFormularBeenEdited();
                    if (result)
                    {
                        saveFormular();
                    } else await Navigation.PopAsync(true);
                };
            }

            this.templateArrayNumber = templateArrayNumber;
            generateTemplate();

            //When a multiselect picker is chosen
            MessagingCenter.Subscribe<AbstractDynamicUI>(this, "MULTISELECT", async (sender) =>
            {
                var items = new List<CheckItem>();
                Dictionary<string, string> valueDictionary = sender.getValueList();
                List<string> valueList = valueDictionary.Values.ToList();
                foreach (string item in valueList)
                {
                    items.Add(new CheckItem { Name = item });
                }

                var multiPage = new SelectMultipleBasePage<CheckItem>(items) { Title = "Check all that apply" };
                await Navigation.PushAsync(multiPage);
            });
        }

        private void generateTemplate()
        {
            DynamicUIFactory dynamicUIFactory = new DynamicUIFactory();
            Dictionary<string, string> templateDictionary = TemplateDictionary.Instance();
            JObject template = JObject.Parse((string)templateDictionary[templateArrayNumber]);
            bool required, multiselect;
            string label, fieldname;

            templateTitle = (string)template["data"]["templatetitle"];
            formtemplateid = (string)template["data"]["formtemplateid"];
            templateversion = (string)template["data"]["templateversion"];
            Title = templateTitle;
            JArray fieldsets = (JArray)template["data"]["fieldsets"];
            for (int i = 0; i < fieldsets.Count; i++)
            {
                /*
                 * FIELDSETS
                 */

                int fieldsetDevicetype = (int)fieldsets[i]["devicetype"];

                //Kigger om fieldsettet skal benyttes i mobilapplikationen. OBS, fremadrettet skal der også tages forbehold for accessread og accessedit
                if (fieldsetDevicetype == 1)
                {
                    //Tilføjer feltsætnavn til dynamicUIList
                    string fieldsetTitle = (string)fieldsets[i]["fieldsettitle"];
                    AbstractDynamicUI guiElementLabel = dynamicUIFactory.getDynamicUI("label");
                    guiElementLabel.setLabel(fieldsetTitle);
                    dynamicUIList.Add(guiElementLabel);


                    JArray fields = (JArray)fieldsets[i]["fields"];
                    for (int ii = 0; ii < fields.Count; ii++)
                    {
                        /*
                         * FIELDS
                         */
                        Dictionary<string, string> valueListDictionary = new Dictionary<string, string>();
                        int fieldDevicetype = (int)fields[ii]["devicetype"];
                        if (fieldDevicetype == 1)
                        {
                            required = (bool)fields[ii]["required"];
                            multiselect = (bool)fields[ii]["multiselect"];
                            label = (string)fields[ii]["label"];
                            fieldname = (string)fields[ii]["fieldname"];

                            //Går igennem valuelist
                            string sourcetype = (string)fields[ii]["sourcetype"];
                            if (sourcetype.Equals("list"))
                            {
                                JArray valueList = (JArray)fields[ii]["valuelist"];
                                for (int iii = 0; iii < valueList.Count; iii++)
                                {
                                    string valueListID = (string)valueList[iii]["id"];
                                    string valueListName = (string)valueList[iii]["name"];
                                    valueListDictionary.Add(valueListID, valueListName);
                                }
                            }

                            //Gets keywords from keywordalias
                            if (sourcetype.Equals("keyword"))
                            {
                                string json = Application.Current.Properties["KEYWORDS"] as string;
                                JArray keywordList = JArray.Parse(json);
                                //JArray keywordList = Application.Current.Properties["KEYWORDS"] as JArray;
                                string keyword = (string)fields[ii]["keywordalias"];

                                for (int iiii = 0; iiii < keywordList.Count; iiii++)
                                {
                                    string alias = (string)keywordList[iiii]["alias"];
                                    if (alias.Equals(keyword))
                                    {
                                        JArray valuesArray = (JArray)keywordList[iiii]["valuesarray"];
                                        foreach (string items in valuesArray)
                                        {
                                            if (!valueListDictionary.ContainsKey(items))
                                            {
                                                valueListDictionary.Add(items, items); //OBS Fejl med samme itemkey
                                            }
                                        }
                                    }
                                }

                            }

                            //Gets the type og GUI element
                            string type = (string)fields[ii]["type"];
                            AbstractDynamicUI guiElement = dynamicUIFactory.getDynamicUI(type);

                            if (guiElement != null)
                            {
                                //Sets the variables of the GUI element
                                guiElement.setRequired(required);
                                guiElement.setMultiSelect(multiselect);
                                guiElement.setID(fieldname);
                                guiElement.setLabel(label);

                                //Sets the valuelist
                                if (valueListDictionary.Any())
                                {
                                    guiElement.setValueList(valueListDictionary);
                                }

                                //Adds the GUI element to a list
                                dynamicUIList.Add(guiElement);
                            }

                        }
                    }
                }
            }

            if (dynamicUIList.Any())
            {
                generateUIFromList();
            }


        }

        private void generateUIFromList()
        {
            foreach (AbstractDynamicUI element in dynamicUIList)
            {
                View guiViewElement = element.getViewElement();

                if (guiViewElement != null)
                {
                    //Tjekker om viewelement er af typen LabelElement
                    if (!(guiViewElement.GetType() == typeof(Label)))
                    {
                        Label fieldname;
                        string label = element.getLabel();
                        if (element.getRequired() == true)
                        {
                            var fs = new FormattedString();
                            fs.Spans.Add(new Span { Text = label, ForegroundColor = Color.Black });
                            fs.Spans.Add(new Span { Text = "*", ForegroundColor = Color.Red });
                            fs.Spans.Add(new Span { Text = ":", ForegroundColor = Color.Black });
                            fieldname = new Label { Margin = new Thickness(10, 0, 10, 0) };
                            fieldname.FormattedText = fs;
                        }
                        else
                        {
                            fieldname = new Label { Margin = new Thickness(10, 0, 10, 0) };
                            fieldname.Text = label + ":";
                        }
                        FormularLayout.Children.Add(fieldname);
                    }
                    FormularLayout.Children.Add(guiViewElement);
                }
            }

            Content = FormularRootView;
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image imageClicked = (Image)sender;
            string templateArrayNumber = imageClicked.ClassId;

            Navigation.PushAsync(new Formular(templateArrayNumber));
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            var overlay = new AbsoluteLayout();
            var content = new StackLayout();
            var loadingIndicator = new ActivityIndicator();
            AbsoluteLayout.SetLayoutFlags(content, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(content, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(loadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(loadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            overlay.Children.Add(content);
            overlay.Children.Add(loadingIndicator);

            foreach (AbstractDynamicUI element in dynamicUIList)
            {
                element.Save();
            }

            PostFormular postFormular = new PostFormular(dynamicUIList, formtemplateid, templateversion);
            postFormular.Post();

        }

        private async Task<bool> hasFormularBeenEdited()
        {
            bool hasFormularBeenEdited = false;
            foreach (AbstractDynamicUI element in dynamicUIList)
            {
                element.Save();
            }

            foreach (AbstractDynamicUI element in dynamicUIList)
            {
                View view = element.getViewElement();
                if (view != null)
                {
                    if (view.GetType() != typeof(Label))
                    {
                        if (view.GetType() != typeof(DatePicker))
                        {
                            if (element.getValue() != null)
                            {
                                if (!(element.getValue().Equals("")))
                                {
                                    hasFormularBeenEdited = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return hasFormularBeenEdited;
        }

        private async void saveFormular()
        {
            var answer = await DisplayAlert("Vil du gemme dine ændringer?", null, "Ja", "Nej");
            //This is where a method should be made to save the dynamicUIList to a LIST.

            if (answer)
            {
                History history = new History();
                history.loadList(dynamicUIList);
            }

            else
            {
                await Navigation.PopAsync(true);
            }
            
        }
    }
}