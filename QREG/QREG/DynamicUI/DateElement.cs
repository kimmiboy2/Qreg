using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class DateElement : AbstractDynamicUI
    {
        DatePicker datepicker;
        public override View getViewElement()
        {
            datepicker = new DatePicker();
            return datepicker;
        }

        public override void Save()
        {
            setValue(datepicker.Date.ToString());
        }
    }
}
