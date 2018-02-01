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
        public override View getViewElement()
        {
            return new Editor();
        }
    }
}
