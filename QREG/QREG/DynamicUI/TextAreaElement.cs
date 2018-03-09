using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class TextAreaElement : AbstractDynamicUI
    {
        Editor editor;
        public override View getViewElement()
        {
            editor = new Editor();
            editor.HeightRequest = 150;

            ScrollView scrollview = new ScrollView { Content = editor };
            Frame frame = new Frame { Content = scrollview, Margin = new Thickness(10,0,10,0)};
            return frame;

            //return new Editor();
        }

        public override void Save()
        {
            setValue(editor.Text);
        }
    }
}
