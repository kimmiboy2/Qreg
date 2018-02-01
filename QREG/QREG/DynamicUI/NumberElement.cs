using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class NumberElement : AbstractDynamicUI
    {
        public override View getViewElement()
        {
            Entry entry = new Entry { Keyboard = Keyboard.Numeric };
            return entry;
        }
    }
}
