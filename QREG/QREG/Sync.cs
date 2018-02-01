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
        int i = 0;
        int templateCount;
        JSONFetcher jsonFetcher = new JSONFetcher();
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        public Sync()
        {
            server = Application.Current.Properties["SERVER"] as string;
            path = Application.Current.Properties["DATABASE_DEVIATION"] as string;

            //When array of templates are done loading
            MessagingCenter.Subscribe<JSONFetcher>(this, "TEMPLATES_LOADED", (sender) =>
            {
                MessagingCenter.Unsubscribe<JSONFetcher>(this, "TEMPLATES_LOADED");
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
                MessagingCenter.Unsubscribe<JSONFetcher>(this, "JSON_ACTIONS_GET_DEPTNAMES_LOADED");
                downloadIcons();
            });

            //When each template are done lading
            MessagingCenter.Subscribe<JSONFetcher, int>(this, "TEMPLATE_LOADED", (sender, arg) =>
            {
                if (i == templateCount - 2)
                {
                    MessagingCenter.Unsubscribe<JSONFetcher, int>(this, "TEMPLATE_LOADED");
                    downloadKeyWords();
                    return;
                }

                i = arg;
                downloadTemplate();
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

            JArray templates = Application.Current.Properties["TEMPLATES"] as JArray;
            templateCount = templates.Count;
            
            string templateID = (string)templates[i]["id"];
            string url = String.Format("{0}{1}HandleFormTemplate?OpenAgent", server, path);
            parameters.Clear();
            parameters.Add("action", "loadcomplete");
            parameters.Add("templateid", templateID);
            jsonFetcher.initWithUrl(url, POST, "TEMPLATE_DATA", parameters);
            jsonFetcher.startJSONFetch();                  
        }

        private void downloadKeyWords()
        {
            string url = String.Format("{0}{1}HandleKeyword?OpenAgent", server, path);
            parameters.Clear();
            parameters.Add("action", "getlist");
            jsonFetcher.initWithUrl(url, POST, "JSON_ACTION_GET_KEYWORDS", parameters);
            jsonFetcher.startJSONFetch();
        }

        private void downloadDeptPeople()
        {
            string url = String.Format("{0}{1}GetPicklistValues?OpenAgent", server, path);
            parameters.Clear();
            parameters.Add("action", "departmentnames");
            jsonFetcher.initWithUrl(url, POST, "JSON_ACTIONS_GET_DEPTNAMES", parameters);
            jsonFetcher.startJSONFetch();
        }

        //Get icons
        private void downloadIcons()
        {
            //Dictionary<string, object> templateDictionary = TemplateDictionary.Instance();
            //templateDictionary.TryGetValue("0", out object template);

            //JObject jsonTemplate = JObject.Parse(template.ToString());

            //string iconUrl = (string)jsonTemplate["templateiconurl"];

            openNewWindow();

            
        }

        private void openNewWindow()
        {
            MessagingCenter.Send<Sync>(this, "LoginSucceded");
        }

    }
}
