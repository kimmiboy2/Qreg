using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QREG
{
    class TemplateDictionary
    {
        private TemplateDictionary()
        {

        }

        private static Dictionary<string, object> instance = null;
        private static readonly object _lock = new object();

        public static Dictionary<string,object> Instance()
        {
            lock (_lock)
            {
                if(instance == null)
                {
                    instance = new Dictionary<string, object>();
                }

                return instance;
            }
        }
    }
}
