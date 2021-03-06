﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using QREG.Utilities;

namespace QREG.DynamicUI
{
    class ComboElement : AbstractDynamicUI
    {
        Picker picker;
        public override View getViewElement()
        {

            bool multiselect = getMultiSelect();
            if (multiselect == false || multiselect == true)
            {
                picker = new Picker();
                picker.BackgroundColor = Color.White;
                picker.Title = "Vælg";

                Dictionary<string, string> valueDictionary = getValueList();
                if (valueDictionary != null)
                {
                    List<string> valueList = valueDictionary.Values.ToList();
                    picker.ItemsSource = valueList;
                }



                return picker;

            }
            //else if (multiselect == true)
            //{
            //    Label label = new Label { Text = "Vælg" };

            //    //Adds onTap event
            //    var tapGestureRecognizer = new TapGestureRecognizer();
            //    tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            //    label.GestureRecognizers.Add(tapGestureRecognizer);

            //    return label;
            //}
            else return null;
        }

        public override void Save()
        {
            if (!(picker.SelectedIndex == -1) || picker == null)
            {
                string value = picker.Items[picker.SelectedIndex];
                setValue(value);
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //Sender besked til Formular.class
            MessagingCenter.Send<AbstractDynamicUI>(this, "MULTISELECT");
        }
    }
}
