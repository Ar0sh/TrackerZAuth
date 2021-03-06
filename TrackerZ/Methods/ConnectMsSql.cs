using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerZ.Methods
{
    internal class ConnectMsSql
    {
        protected internal static string ConnString()
        {
            string connString = "Server=192.168.1.161; database=TrackerZ; UID=buguser; password=123456";
            return connString;
        }
    }
}
