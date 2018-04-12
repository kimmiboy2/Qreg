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

            bool isUserLoggedIn;
            if (Properties.ContainsKey("isUserLoggedIn"))
            {
                isUserLoggedIn = (bool)Properties["isUserLoggedIn"];
            }

            else isUserLoggedIn = false;

            if (!isUserLoggedIn)
            {
                MainPage = new NavigationPage(new QREG.MainPage());
            }

            else
            {
                MainPage = new NavigationPage(new QREG.MainMenuPage());
            }           
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

            
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            
        }
    }
}