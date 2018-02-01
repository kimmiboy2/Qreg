using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class DepartmentnamepickerElement : AbstractDynamicUI
    {
        public override View getViewElement()
        {
            Label label = new Label { Text = "Vælg" };

            //Adds onTap event
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            label.GestureRecognizers.Add(tapGestureRecognizer);

            return label;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //Sender besked til Formular.class
            MessagingCenter.Send<AbstractDynamicUI>(this, "MULTISELECT");
        }
    }
}
