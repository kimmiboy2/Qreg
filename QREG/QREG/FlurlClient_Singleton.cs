using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;


namespace QREG
{
    class FlurlClient_Singleton
    {
        private FlurlClient_Singleton()
        {

        }

        private static FlurlClient instance = null;
        private static readonly object _lock = new object();

        public static FlurlClient GetInstance()
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = new FlurlClient();
                }

                return instance;
            }
        }

        public static void DisposeInstance()
        {
            instance = null;
        }

        public static void setFlurlClient(FlurlClient client)
        {
            instance = client;
        }
    }
    
}
