using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QREG.DynamicUI
{
    class DynamicUIFactory
    {
        public AbstractDynamicUI getDynamicUI(string type)
        {
            if(type == null)
            {
                return null;
            }

            if (type.Equals("text"))
            {
                return new EntryElement();
            }

            if (type.Equals("textarea"))
            {
                return new TextAreaElement();
            }

            if (type.Equals("radio"))
            {
                return new RadioElement();
            }

            if (type.Equals("combo"))
            {
                return new ComboElement();
            }

            if (type.Equals("departmentnamepicker"))
            {
                return new EntryElement();
            }

            if (type.Equals("number"))
            {
                return new NumberElement();
            }

            if (type.Equals("attachmentgrid"))
            {
                return new AttachmentgridElement();
            }

            if (type.Equals("cprnr"))
            {
                return new CprnrElement();
            }

            if (type.Equals("editor"))
            {
                return new EditorElement();
            }

            if (type.Equals("multidisplay"))
            {
                return new MultiDisplayElement();
            }

            if (type.Equals("display"))
            {
                return new DisplayElement();
            }

            if (type.Equals("checkbox"))
            {
                return new CheckboxElement();
            }

            if (type.Equals("date"))
            {
                return new DateElement();
            }

            return null;
        }
    }
}
