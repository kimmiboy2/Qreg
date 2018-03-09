using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class CprnrElement : AbstractDynamicUI
    {
        Entry entry;
        public override View getViewElement()
        {
            entry = new Entry { Keyboard = Keyboard.Numeric};
            return entry;
        }

        public override void Save()
        {
            setValue(entry.Text);
        }
    }
}
