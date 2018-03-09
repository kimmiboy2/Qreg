using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    class EntryElement : AbstractDynamicUI
    {
        Entry entry;
        Frame frame;
        public override View getViewElement()
        {
            entry = new Entry();
            if (getRequired() == true)
            {
                entry.TextChanged += Entry_TextChanged;
                entry.Placeholder = "Påkrævet";
                frame = new Frame { Content = entry, Margin = new Thickness(10, 0, 10, 0), OutlineColor = Color.Red, BackgroundColor = Color.Pink };
                frame.HasShadow = false;
                return frame;
            }

            else return entry;

        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!entry.Text.Equals(""))
            {
                frame.OutlineColor = Color.White;
                frame.BackgroundColor = Color.White;
            }

            if (entry.Text.Equals(""))
            {
                frame.OutlineColor = Color.Red;
                frame.BackgroundColor = Color.Pink;
            }
        }

        public override void Save()
        {
            setValue(entry.Text);
        }
    }
}
