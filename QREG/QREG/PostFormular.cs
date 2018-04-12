using Flurl.Http;
using Newtonsoft.Json.Linq;
using QREG.DynamicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG
{
    class PostFormular
    {
        List<AbstractDynamicUI> dynamicUIList;
        Dictionary<string, string> formularDictionary = new Dictionary<string, string>();
        string formtemplateid, templateversion;
        bool allRequiredFieldsFilled = true;
        FlurlClient flurlClient;

        public PostFormular(List<AbstractDynamicUI> dynamicUIList, string formtemplateid, string templateversion)
        {
            this.dynamicUIList = dynamicUIList;
            this.formtemplateid = formtemplateid;
            this.templateversion = templateversion;
        }

        public async void Post()
        {
            foreach (AbstractDynamicUI element in dynamicUIList)
            {
                if (!(element.GetType() == typeof(Label)))
                {
                    if(element.getRequired() == true)
                    {
                        if (element.getValue() != null && !element.getValue().Equals(""))
                        {
                            formularDictionary.Add(element.getID(), element.getValue());
                            allRequiredFieldsFilled = true;
                        }

                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Udfyld alle påkrævede felter", null, "OK");
                            allRequiredFieldsFilled = false;
                            return;
                        }   
                    }

                    else
                    {
                        if (element.getValue() != null)
                        {
                            formularDictionary.Add(element.getID(), element.getValue());
                        }
                    }
                }
            }

            if (allRequiredFieldsFilled)
            {
                Authenticate();
            }
        }

        private void Authenticate()
        {
            performAuthentication();

        }

        private async void performAuthentication()
        {
            flurlClient = FlurlClient_Singleton.GetInstance();
            //loadCustomerPath
            string firma = Application.Current.Properties["firma"] as string;
            string urlCus = String.Format("http://myqreg.dk/qreg/{0}", firma);

            Task<string> getStringTask = urlCus.WithClient(flurlClient).GetStringAsync();
            string responseString = await getStringTask;

            if(responseString != null)
            {
                string server = Application.Current.Properties["SERVER"] as string;
                string brugernavn = Application.Current.Properties["brugernavn"] as string;
                string password = Application.Current.Properties["password"] as string;
                string url = String.Format("{0}/names.nsf?login&username={1}&password={2}", server, brugernavn, password);

                Task<string> getStringTask1 = url.WithClient(flurlClient).GetStringAsync();
                string responseString1 = await getStringTask1;
                if(responseString1 != null)
                {
                    FlurlClient_Singleton.setFlurlClient(flurlClient);
                    PostToURL();
                }
            }
        }

        private async void PostToURL()
        {
            formularDictionary.Add("action", "save");
            formularDictionary.Add("formtemplateid", formtemplateid);
            formularDictionary.Add("templateversion", templateversion);

            string url = "https://e-dok.rm.dk/qreg/hoveim/qreg.nsf/HandleDeviation?OpenAgent";
            string responseString = await url.WithClient(flurlClient)
                        .PostUrlEncodedAsync(formularDictionary).ReceiveString();

            JObject responseJSON = JObject.Parse(responseString);
            bool success = (bool)responseJSON["success"];

            if (success)
            {
                await App.Current.MainPage.DisplayAlert("Formular indsendt", null, "OK");
                await App.Current.MainPage.Navigation.PopAsync();
            }

            else
            {
                await App.Current.MainPage.DisplayAlert("Formular ikke indsendt. Prøv igen.", null, "OK");
            }
        }
    }
}
