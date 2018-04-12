using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace QREG
{
    class JSONFetcher
    {
        string url, method, assignment;
        FlurlClient _flurlClient = FlurlClient_Singleton.GetInstance();
        string responseString;
        int i = Constants.templateCounter;
        Dictionary<string, string> parameters;
        Sync sync;

        public JSONFetcher(Sync sync)
        {
            this.sync = sync;
        }

        public string initWithUrl(string url, string method, string assignment, Dictionary<string, string> parameters)
        {
            this.url = url;
            this.assignment = assignment;
            this.parameters = parameters;


            if (!method.Equals(""))
            {
                this.method = method;
            }

            else this.method = "JSONFETCHER_METHOD_GET";
            return null;
        }


        internal async void startJSONFetch()
        {
            if (method.Equals("JSONFETCHER_METHOD_GET"))
            {
                responseString = await url.WithClient(_flurlClient).GetStringAsync();
                if (responseString != null)
                {
                    if (assignment.Equals("LOGIN"))
                    {
                        //if (!responseString.Contains("names.nsf?Login"))
                        //{
                        //    login = new Login();
                        //    login.loadSessionData();
                        //}
                        //else
                        //{
                        //    Application.Current.Properties.Clear();
                        //    await Application.Current.MainPage.DisplayAlert("Login fejl", "Forkert brugernavn eller adgangskode. Prøv igen.", "OK");
                        //    disposeFlurlClient();
                        //}
                        MessagingCenter.Send<JSONFetcher, string>(this, "loginResponseString", responseString);
                    }

                    else saveData(responseString, assignment);
                }
            }

            else if (method.Equals("JSONFETCHER_METHOD_POST"))
            {

                if (assignment.Equals("TEMPLATE_DATA"))
                {
                    parameters.TryGetValue("action", out string action);
                    parameters.TryGetValue("templateid", out string templateID);
                    responseString = await url.WithClient(_flurlClient)
                        .PostUrlEncodedAsync(new { action = action, templateid = templateID })
                        .ReceiveString();
                }

                else
                {
                    parameters.TryGetValue("action", out string parameter);
                    responseString = await url.WithClient(_flurlClient)
                        .PostUrlEncodedAsync(new { action = parameter })
                        .ReceiveString();
                }

                saveData(responseString, assignment);
            }
        }

        private void saveData(string responseString, string assignment)
        {
            JObject responseJSON = null;
            responseJSON = JObject.Parse(responseString);

            if (assignment.Equals("JSON_ACTION_LOAD_CUST_PATH"))
            {
                if (responseJSON != null)
                {
                    Application.Current.Properties["DATABASE_DEVIATION"] = (string)responseJSON["path"];
                    Application.Current.Properties["SERVER"] = (string)responseJSON["server"];
                    MessagingCenter.Send<JSONFetcher>(this, "CUST_PATH_LOADED");
                }

                else
                {
                    //Did not receive data
                }
            }

            else if (assignment.Equals("JSON_ACTIONS_GET_DEPTNAMES"))
            {
                Application.Current.Properties["DEPTPEOPLE"] = responseJSON["data"].ToString(); ;
                //sync.downloadIcons();
                //MessagingCenter.Send<JSONFetcher>(this, "JSON_ACTIONS_GET_DEPTNAMES_LOADED");
            }

            else if (assignment.Equals("JSON_ACTION_GET_KEYWORDS"))
            {
                Application.Current.Properties["KEYWORDS"] = responseJSON["data"].ToString();
                Sync sync = new Sync();
                sync.downloadDeptPeople();
                //MessagingCenter.Send<JSONFetcher>(this, "JSON_ACTION_GET_KEYWORDS_LOADED");
            }

            else if (assignment.Equals("TEMPLATE_DATA"))
            {
                Dictionary<string, string> templateDictionary = TemplateDictionary.Instance();
                string count = templateDictionary.Count().ToString();
                templateDictionary.Add(count, responseString);
                sync.downloadTemplate();
            }

            else if (assignment.Equals("TEMPLATES_DATA"))
            {
                Application.Current.Properties["TEMPLATES"] = responseJSON["data"].ToString();
                MessagingCenter.Send<JSONFetcher>(this, "TEMPLATES_LOADED");
            }

            else if (assignment.Equals(Constants.LOAD_SESSION_DATA))
            {
                Application.Current.Properties["SESSION_COMMON_USERNAME"] = (string)responseJSON["session"]["commonUserName"];
                Application.Current.Properties["SESSION_USERNAME"] = (string)responseJSON["session"]["userName"];
                Application.Current.Properties["DATABASE_UPLOAD"] = (string)responseJSON["session"]["uploadDatabase"];
                //MessagingCenter.Send<JSONFetcher>(this, Constants.SESSION_DATA_LOADED);
            }
        }

        internal void disposeFlurlClient()
        {
            FlurlClient_Singleton.DisposeInstance();
            _flurlClient = FlurlClient_Singleton.GetInstance();
        }
    }
}
