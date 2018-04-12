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

//[assembly: ExportRenderer(typeof(Editor), typeof(QREG.Droid.CustomEntryRenderer))]

namespace QREG.Droid
{
    class CustomTextAreaRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                return;
            }

            var nativeEditTextField = (Android.Widget.EditText)this.Control;

            nativeEditTextField.SetHintTextColor(Android.Graphics.Color.White);

            const int ID = Resource.Drawable.entry_border;
            var drawable = this.Context.Resources.GetDrawable(ID);
            nativeEditTextField.SetBackgroundDrawable(drawable);
        }
    }
}