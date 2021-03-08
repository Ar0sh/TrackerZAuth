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
            string connection = ConnectMsSql.ConnString();
            return new SqlConnection(connection);
        }
        public static void Add(string category)
        {
            using SqlConnection conn = GetConnection();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"insert into BaseCategory(cat_name) VALUES (@Category)", conn);
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Category", SqlDbType.NVarChar)
                {
                    Value = category.Trim()
                };
                cmd.Parameters.Add(param[0]);
                cmd.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
        }
        public void GetBaseCat()
        {
            _category = new List<Category>();
            _baseCat = new List<SelectListItem>();
            using SqlConnection conn = GetConnection();
            try
            {
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
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
        }

        public static void Remove(string catid)
        {
            using SqlConnection conn = GetConnection();
            try
            {
                conn.Open();
                SqlCommand check = new SqlCommand($"update IncidentList set catid = 'BC0000000022' where catid = '{catid}'", conn);
                check.ExecuteNonQuery();
                SqlCommand cmd = new SqlCommand("delete from BaseCategory where catid = @CatID", conn);
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CatID", SqlDbType.NVarChar)
                {
                    Value = catid.Trim()
                };
                cmd.Parameters.Add(param[0]);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
        }

        public IEnumerable<Category> GetBaseCategory()
        {
            GetBaseCat();
            return _category;
        }
    }
}
