using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QREG.DynamicUI
{
    abstract class AbstractDynamicUI
    {
        bool multiSelect = false;
        bool required = false;

        string id, label;

        public Dictionary<string, string> valueDictionary;

        //Gets the viewElement associated with the derived class
        public abstract View getViewElement();

        //Gets the ID of the element in order to set the key when sending to server
        public string getID()
        {
            return this.id;
        }

        //Sets ID of the element correspeonding to "fieldname" in JSONTemplate
        public void setID(string id)
        {
            this.id = id;
        }


        //Sets if this element should be required or not
        public void setRequired(bool required)
        {
            this.required = required;
        }

        //Sets if multiselect should be possible or not
        public void setMultiSelect(bool multiSelect)
        {
            this.multiSelect = multiSelect;
        }

        //Gets multiselect
        public bool getMultiSelect()
        {
            return this.multiSelect;
        }
        //Sets the label 
        public void setLabel(string label)
        {
            this.label = label;
        }

        public string getLabel()
        {
            return this.label;
        }

        //Sets the valueList in JSONTemplate
        public void setValueList(Dictionary<string, string> valueList)
        {
            this.valueDictionary = valueList;
        }

        public Dictionary<string,string> getValueList()
        {
            return this.valueDictionary;
        }
    }
}
