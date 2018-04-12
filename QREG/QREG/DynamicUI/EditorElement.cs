using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class EditorElement : AbstractDynamicUI
    {
        Editor editor;
        public override View getViewElement()
        {
            editor = new Editor();
            return editor;
        }



        public override void Save()
        {
            setValue(editor.Text);
        }
    }
}
