using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace QREG
{
    public partial class App : Application
    {
        string BRUGERNAVN_KEY = "Brugernavn";
        string FIRMA_KEY = "Firma";
        string PASSWORD_KEY = "Password";

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new QREG.MainPage());
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