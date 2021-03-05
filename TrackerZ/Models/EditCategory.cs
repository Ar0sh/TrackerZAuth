using Microsoft.AspNetCore.Mvc.Rendering;
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
        private List<Category> _category;
        private List<SelectListItem> _baseCat;
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
        public void GetBaseCat()
        {
            _category = new List<Category>();
            _baseCat = new List<SelectListItem>();
            try
            {
                using SqlConnection conn = GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from BaseCategory order by cat_name asc", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _category.Add(new Category()
                        {
                            Id = (int)reader[0],
                            CatId = reader[1].ToString(),
                            CatName = reader[2].ToString()
                        });
                        _baseCat.Add(new SelectListItem { Value = reader[0].ToString(), Text = reader[2].ToString() });
                    };
                }
                conn.Close();
            }
            catch
            {

            }
        }

        public IEnumerable<Category> GetBaseCategory()
        {
            GetBaseCat();
            return _category;
        }
    }
}
