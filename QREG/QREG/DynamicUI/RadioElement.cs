using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class RadioElement : AbstractDynamicUI
    {
        public override View getViewElement()
        {
            Picker picker = new Picker();
            picker.Title = "Vælg";

            if (valueDictionary != null)
            {
                List<string> valueList = valueDictionary.Values.ToList();
                picker.ItemsSource = valueList;
            }

            return picker;
        }
    }
}
