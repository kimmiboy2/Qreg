using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG
{
    class Login : IJSONFetcherResponse
    {
        string firma, brugernavn, password;
        string GET = "JSONFETCHER_METHOD_GET";
        string server, path;
        JSONFetcher jsonfetcher = new JSONFetcher();

        public Login()
        {
            MessagingCenter.Subscribe<JSONFetcher, string>(this, "loginResponseString", (sender, arg) =>
            {
                if (!arg.Contains("names.nsf?Login"))
                {
                    MessagingCenter.Unsubscribe<JSONFetcher, string>(this, "loginResponseString");
                    Application.Current.MainPage.DisplayAlert("Login success", "Logget ind", "OK");
                    loadSessionData();
                }

                else
                {
                    Application.Current.Properties.Clear();
                    Application.Current.MainPage.DisplayAlert("Login fejl", "Forkert brugernavn eller adgangskode. Prøv igen.", "OK");
                    jsonfetcher.disposeFlurlClient();
                }
            });

            MessagingCenter.Subscribe<JSONFetcher>(this, "CUST_PATH_LOADED", (sender) =>
            {
                MessagingCenter.Unsubscribe<JSONFetcher, string>(this, "CUST_PATH_LOADED");
                performAuthentification();
            });
        }

        internal void loadLoginInfo()
        {
            firma = Application.Current.Properties["firma"] as string;
            brugernavn = Application.Current.Properties["brugernavn"] as string;
            password = Application.Current.Properties["password"] as string;
        }

        internal void loadCustomerPath()
        {
            string url = String.Format("http://myqreg.dk/qreg/{0}", firma);
            string method = GET;

            string assignment = "JSON_ACTION_LOAD_CUST_PATH";

            jsonfetcher.initWithUrl(url, method, assignment, null);
            jsonfetcher.startJSONFetch();
        }

        private void performAuthentification()
        {
            server = Application.Current.Properties["SERVER"] as string;
            string url = String.Format("{0}/names.nsf?login&username={1}&password={2}", server, brugernavn, password);
            jsonfetcher.initWithUrl(url, "", "LOGIN", null);
            jsonfetcher.startJSONFetch();
        }

        private void loadSessionData()
        {
            path = Application.Current.Properties["DATABASE_DEVIATION"] as string;
            string url = String.Format("{0}{1}appsession?OpenAgent", server, path);
            jsonfetcher.initWithUrl(url, "", Constants.LOAD_SESSION_DATA, null);
            jsonfetcher.startJSONFetch();

            Sync sync = new Sync();
            sync.downloadTemplates();
        }

    }
}
