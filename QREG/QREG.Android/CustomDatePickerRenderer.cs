using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.ComponentModel;

//[assembly: ExportRenderer(typeof(Xamarin.Forms.DatePicker), typeof(QREG.Droid.CustomDatePickerRenderer))]
namespace QREG.Droid
{

    class CustomDatePickerRenderer
    {
        public class OptionalDatePickerRenderer : DatePickerRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
            {
                base.OnElementChanged(e);
                SetText();
            }

            protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                base.OnElementPropertyChanged(sender, e);
                if (e.PropertyName == "" || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
                {
                    SetText();
                }
            }

            void SetText()
            {
                // date currently set on the optional date picker (date known to model)
                var date = Element.Date;
                if (date == Element.MinimumDate)
                    Control.Text = "";
            }
        }
    }
}