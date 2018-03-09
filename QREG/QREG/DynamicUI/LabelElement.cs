using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class LabelElement : AbstractDynamicUI
    {
        public override View getViewElement()
        {
            Label label = new Label { Margin = new Thickness(0, -5, 0, -5)};
            label.BackgroundColor = Color.DimGray;
            label.TextColor = Color.White;
            label.FontSize = 20;
            label.FontAttributes = FontAttributes.Bold;
            label.Text = " " + getLabel();

            return label;
        }

        public override void Save()
        {
            
        }
    }
}
