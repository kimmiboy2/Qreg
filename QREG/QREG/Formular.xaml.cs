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
    public partial class Formular : ContentPage
    {
        JSONFetcher jsonFetcher = new JSONFetcher();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        FlurlClient flurlClient = FlurlClient_Singleton.GetInstance();
        List<AbstractDynamicUI> dynamicUIList = new List<AbstractDynamicUI>();
        string overskrift, afvigelse, korrigerendehandling, forslag, handleplan, kategori, url;
        string templateArrayNumber;
        string templateTitle;

        List<Element> elementsList = new List<Element>();

        public Formular(string templateArrayNumber)
        {
            InitializeComponent();
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
                    items.Add(new CheckItem{Name = item});
                }

                var multiPage = new SelectMultipleBasePage<CheckItem>(items) { Title = "Check all that apply" };
                await Navigation.PushAsync(multiPage);
            });
        }

        private void generateTemplate()
        {
            DynamicUIFactory dynamicUIFactory = new DynamicUIFactory();
            Dictionary<string, object> templateDictionary = TemplateDictionary.Instance();
            JObject template = JObject.Parse((string)templateDictionary[templateArrayNumber]);
            bool required, multiselect;
            string label, fieldname;

            templateTitle = (string)template["data"]["templatetitle"];
            JArray fieldsets = (JArray) template["data"]["fieldsets"];
            for (int i = 0; i < fieldsets.Count; i++)
            {
                /*
                 * FIELDSETS
                 */

                int fieldsetDevicetype = (int)fieldsets[i]["devicetype"];

                //Kigger om fieldsettet skal benyttes i mobilapplikationen. OBS, fremadrettet skal der også tages forbehold for accessread og accessedit
                if(fieldsetDevicetype == 1)
                {
                    string fieldsetTitle = (string)fieldsets[i]["fieldsettitle"];

                    JArray fields = (JArray) fieldsets[i]["fields"];
                    for(int ii = 0; ii < fields.Count; ii++)
                    {
                        /*
                         * FIELDS
                         */
                        Dictionary<string, string> valueListDictionary = new Dictionary<string, string>();
                        int fieldDevicetype = (int)fields[ii]["devicetype"];
                        if(fieldDevicetype == 1)
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
                                JArray keywordList = Application.Current.Properties["KEYWORDS"] as JArray;
                                string keyword = (string)fields[ii]["keywordalias"];
                                
                                for (int iiii = 0; iiii < keywordList.Count; iiii++)
                                {
                                    string alias = (string)keywordList[iiii]["alias"];
                                    if (alias.Equals(keyword))
                                    {
                                        JArray valuesArray = (JArray)keywordList[iiii]["valuesarray"];
                                        foreach (string items in valuesArray)
                                        {
                                            valueListDictionary.Add(items, items);
                                        }
                                    }
                                }

                            }

                            //Gets the type og GUI element
                            string type = (string)fields[ii]["type"];
                            AbstractDynamicUI guiElement = dynamicUIFactory.getDynamicUI(type);

                            if(guiElement != null)
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
            foreach(AbstractDynamicUI element in dynamicUIList)
            {         
                View guiViewElement = element.getViewElement();

                if(guiViewElement != null)
                {
                    string label = element.getLabel();
                    Label fieldname = new Label();
                    fieldname.Text = label;
                    FormularLayout.Children.Add(fieldname);
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

            /**
            overskrift = OverskriftEntry.Text;
            afvigelse = AfvigelseEntry.Text;
            korrigerendehandling = KorrigerendeHandlingerEntry.Text;
            forslag = ForslagEntry.Text;
            handleplan = HandleplanEntry.Text;
            kategori = KategoriPicker.Items[KategoriPicker.SelectedIndex];
            url = "https://e-dok.rm.dk/qreg/hoveim/qreg.nsf/HandleDeviation?OpenAgent";

            SendData();

    */
        }

        private async void SendData()
        {

            string responseString = await url.WithClient(flurlClient)
                        .PostUrlEncodedAsync(new
                        {
                            action = "save",
                            formtemplateid = "D3DC808668768B6BC1257DCC003768C3",
                            templateversion = 4,
                            qreg_title = overskrift,
                            beskrivelse = afvigelse,
                            korrigerende = korrigerendehandling,
                            forslag = forslag,
                            handleplan = handleplan,
                            kateg = kategori
                        })
                        .ReceiveString();

            JObject responseJSON = JObject.Parse(responseString);
            bool success = (bool)responseJSON["success"];
            if (success)
            {
                await DisplayAlert("Formular indsendt", null, "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Formular ikke indsendt", null, "OK");
            }

            
        }
    }
}