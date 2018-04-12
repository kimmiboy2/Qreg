using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QREG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //Clears properties
            Application.Current.Properties.Clear();

            //Clears templateDictionary
            TemplateDictionary.setDictionary(new Dictionary<string, string>());

            //Dispose FlurlClient
            FlurlClient_Singleton.DisposeInstance();

            Application.Current.SavePropertiesAsync();

            //Returns to loginPage
            Application.Current.MainPage.Navigation.PushAsync(new MainPage());

            //Unsubscribe to all MessagingCenters
            MessagingCenter.Unsubscribe<JSONFetcher, string>(this, "loginResponseString");
            MessagingCenter.Unsubscribe<JSONFetcher, string>(this, "CUST_PATH_LOADED");
            MessagingCenter.Unsubscribe<JSONFetcher>(this, "JSON_ACTION_GET_KEYWORDS_LOADED");
            MessagingCenter.Unsubscribe<JSONFetcher>(this, "TEMPLATES_LOADED");


            //Clears navigation stack
            var existingPages = Navigation.NavigationStack.ToList();
            foreach (var page in existingPages)
            {
                Navigation.RemovePage(page);
            }



        }
    }
}
