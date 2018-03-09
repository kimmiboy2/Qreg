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
        Picker picker;
        public override View getViewElement()
        {
            picker = new Picker();
            picker.Title = "Vælg";

            if (valueDictionary != null)
            {
                List<string> valueList = valueDictionary.Values.ToList();
                picker.ItemsSource = valueList;
            }

            return picker;
        }

        public override void Save()
        {
            if(picker.SelectedItem != null)
            {
                setValue(picker.SelectedItem.ToString());
            }
            
        }
    }
}
