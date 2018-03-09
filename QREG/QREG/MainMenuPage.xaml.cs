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
        Grid grid;
        Frame frame;
        List<Frame> imageList = new List<Frame>();
        public MainMenuPage()
        {
            InitializeComponent();
            loadIcons();
            App.Current.Properties["isUserLoggedIn"] = true;

        }

        private void loadIcons()
        {
            Dictionary<string, object> templateDictionary = TemplateDictionary.Instance();

            int left = 0;
            int top = 0;

            

            for (int i = 0; i < templateDictionary.Count; i++)
            {
                StackLayout stackLayout = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start};
                Label label = new Label();

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
                label.Text = (string)templateJSON["data"]["templatetitle"];
                label.TextColor = Color.Black;
                label.HorizontalOptions = LayoutOptions.Center;

                stackLayout.Children.Add(iconImage);
                stackLayout.Children.Add(label);

                frame = new Frame { Content = stackLayout, Margin = new Thickness(2, 2, 2, 2), ClassId = i.ToString(), StyleId = (string)templateJSON["data"]["templatetitle"] };
                frame.HasShadow = true;

                //Adds onTap event
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                frame.GestureRecognizers.Add(tapGestureRecognizer);

                imageList.Add(frame);
            }

            //Sort alphabetically
            List<Frame> sortedFrameList = imageList.OrderBy(o=>o.StyleId).ToList();

            for (int i = 0; i < sortedFrameList.Count; i++)
            {
                frame = sortedFrameList[i];
                MainLayout.Children.Add(frame, left, top);
                left++;

                if (left > 1)
                {
                    top++;
                    left = 0;
                }

            }

            Content = ParentLayout;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Frame imageClicked = (Frame)sender;
            string templateArrayNumber = imageClicked.ClassId;

            Navigation.PushAsync(new Formular(templateArrayNumber));
        }
        
    }
}