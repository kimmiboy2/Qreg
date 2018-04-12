using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QREG.DynamicUI;

namespace QREG
{
    class History 
    {
        List<AbstractDynamicUI> dynamicUIList;


        internal void loadList(List<AbstractDynamicUI> dynamicUIList)
        {
            this.dynamicUIList = dynamicUIList;
        }


    }
}
