using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace QREG
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (App.Current.Properties.ContainsKey("isUserLoggedIn"))
            {
                bool isUserLoggedIn = (bool)App.Current.Properties["isUserLoggedIn"];

                if (isUserLoggedIn)
                {
                    MainPage = new NavigationPage(new QREG.MainMenuPage());
                }
                else MainPage = new NavigationPage(new QREG.MainPage());
            }

            else
            {
                MainPage = new NavigationPage(new MainPage());
            }

            

            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            App.Current.SavePropertiesAsync();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            App.Current.SavePropertiesAsync();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            App.Current.SavePropertiesAsync();
        }
    }
}