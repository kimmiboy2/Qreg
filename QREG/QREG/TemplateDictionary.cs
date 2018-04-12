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

        private static Dictionary<string, string> instance = null;
        private static readonly object _lock = new object();

        public static Dictionary<string,string> Instance()
        {
            lock (_lock)
            {
                if(instance == null)
                {
                    instance = new Dictionary<string, string>();
                }

                return instance;
            }
        }

        public static void setDictionary(Dictionary<string,string> dictionary)
        {
            instance = dictionary;
        }
    }
}
