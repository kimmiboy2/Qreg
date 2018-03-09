using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG
{
    public partial class MainPage : ContentPage
    {
        Login _login;
        public MainPage()
        {
            InitializeComponent();
            _login = new Login();
        }

        private void login_Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["firma"] = firma_Entry.Text;
            Application.Current.Properties["brugernavn"] = brugernavn_Entry.Text;
            Application.Current.Properties["password"] = password_Entry.Text;
            _login.loadLoginInfo();
            _login.loadCustomerPath();
        }
    }
}
