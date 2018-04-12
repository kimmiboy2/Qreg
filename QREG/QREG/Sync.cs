using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.IO;

namespace QREG
{
    class Sync
    {
        string server, path;
        string POST = "JSONFETCHER_METHOD_POST";
        int i;
        int templateCount;
        JSONFetcher jsonFetcher;
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        public Sync()
        {
            jsonFetcher = new JSONFetcher(this);
            server = Application.Current.Properties["SERVER"] as string;
            path = Application.Current.Properties["DATABASE_DEVIATION"] as string;

            //When array of templates are done loading
            MessagingCenter.Subscribe<JSONFetcher>(this, "TEMPLATES_LOADED", (sender) =>
            {
                //MessagingCenter.Unsubscribe<JSONFetcher>(this, "TEMPLATES_LOADED");
                i = 0;
                downloadTemplate();
            });

            //When keywords are done loading
            MessagingCenter.Subscribe<JSONFetcher>(this, "JSON_ACTION_GET_KEYWORDS_LOADED", (sender) =>
            {
                MessagingCenter.Unsubscribe<JSONFetcher>(this, "JSON_ACTION_GET_KEYWORDS_LOADED");
                downloadDeptPeople();
            });

            //When department people are done loading
            MessagingCenter.Subscribe<JSONFetcher>(this, "JSON_ACTIONS_GET_DEPTNAMES_LOADED", (sender) =>
            {
                //MessagingCenter.Unsubscribe<JSONFetcher>(this, "JSON_ACTIONS_GET_DEPTNAMES_LOADED");
                //downloadIcons();
            });
        }

        public void downloadTemplates()
        {
            string url = String.Format("{0}{1}HandleFormTemplate?OpenAgent", server, path);
            parameters.Add("action", "getactivelist");
            jsonFetcher.initWithUrl(url, POST, "TEMPLATES_DATA", parameters);
            jsonFetcher.startJSONFetch();
        }


        public void downloadTemplate()
        {
            string json = Application.Current.Properties["TEMPLATES"] as string;
            JArray templates = JArray.Parse(json);
            //JArray templates = Application.Current.Properties["TEMPLATES"] as JArray;
            templateCount = templates.Count;

            if (i == templateCount - 0)
            {
                MessagingCenter.Unsubscribe<JSONFetcher>(this, "TEMPLATES_LOADED");
                downloadKeyWords();
                return;
            }
            else
            {
                
                string templateID = (string)templates[i]["id"];
                i++;

                string url = String.Format("{0}{1}HandleFormTemplate?OpenAgent", server, path);
                parameters.Clear();
                parameters.Add("action", "loadcomplete");
                parameters.Add("templateid", templateID);
                jsonFetcher.initWithUrl(url, POST, "TEMPLATE_DATA", parameters);
                jsonFetcher.startJSONFetch();
            }                  
        }



        private void downloadKeyWords()
        {
            string url = String.Format("{0}{1}HandleKeyword?OpenAgent", server, path);
            parameters.Clear();
            parameters.Add("action", "getlist");
            jsonFetcher.initWithUrl(url, POST, "JSON_ACTION_GET_KEYWORDS", parameters);
            jsonFetcher.startJSONFetch();
        }

        public void downloadDeptPeople()
        {
            string url = String.Format("{0}{1}GetPicklistValues?OpenAgent", server, path);
            parameters.Clear();
            parameters.Add("action", "departmentnames");
            jsonFetcher.initWithUrl(url, POST, "JSON_ACTIONS_GET_DEPTNAMES", parameters);
            jsonFetcher.startJSONFetch();

            openMainMenu();
        }

        private void openMainMenu()
        {
            Application.Current.Properties["isUserLoggedIn"] = true;
            Application.Current.SavePropertiesAsync();
            Application.Current.MainPage.Navigation.PushAsync(new MainMenuPage());
        }

    }
}
