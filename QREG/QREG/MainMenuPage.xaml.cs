using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QREG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            loadIcons();

        }

        private void loadIcons()
        {
            Dictionary<string, object> templateDictionary = TemplateDictionary.Instance();

            for (int i = 0; i < templateDictionary.Count; i++)
            {
                //Get iconURL
                string templateString = (string)templateDictionary[i.ToString()];
                JObject templateJSON = JObject.Parse(templateString);
                string iconURL = (string)templateJSON["templateiconurl"];

                //Set image to iconURL
                var imageSource = new UriImageSource { Uri = new Uri("https://e-dok.rm.dk/" + iconURL) };
                Image iconImage = new Image();
                iconImage.Source = imageSource;
                iconImage.ClassId = i.ToString();
                iconImage.HeightRequest = 100;
                iconImage.WidthRequest = 100;
                
                //Adds the layout to the contentpage
                MainLayout.Children.Add(iconImage);

                //Adds onTap event
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                iconImage.GestureRecognizers.Add(tapGestureRecognizer);
            }

            Content = ParentLayout;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image imageClicked = (Image)sender;
            string templateArrayNumber = imageClicked.ClassId;

            Navigation.PushAsync(new Formular(templateArrayNumber));
        }
        
    }
}