using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TrackerZ.Methods;

namespace TrackerZ.Models
{
    public class EditCategory : ICategoryRepository
    {
        private static SqlConnection GetConnection()
        {
            string connection = ConnectMySql.ConnString("BugTracker", "buguser", "123456");

            return new SqlConnection(connection);
        }
        public static void Add(string category)
        {
            using SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand cmd =
                new SqlCommand($"insert into BaseCategory(cat_name) VALUES (@Category)", conn);
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Category", SqlDbType.NVarChar)
            {
                Value = category
            };
            cmd.Parameters.Add(param[0]);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
