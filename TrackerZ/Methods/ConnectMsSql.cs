using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerZ.Methods
{
    internal class ConnectMySql
    {
        protected internal static string ConnString(string database, string user, string passwd)
        {
            string connString = "Server=192.168.1.161; database=" + database + "; UID=" + user + "; password=" + passwd;
            return connString;
        }
    }
}
