using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TrackerZ.Methods;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrackerZ.Models
{
    public class BugData : IBugsRepository
    {
        private static readonly DateTime? nullableDateTime = null;
        private Bugs _bug;
        private List<Bugs> _bugs;
        private List<Category> _category;
        private static List<SelectListItem> _statusCat;
        private List<SelectListItem> _baseCat;
        private int _bugCounts;
        //private int _index;

        private static SqlConnection GetConnection()
        {
            string connection = ConnectMySql.ConnString("BugTracker", "buguser", "123456");
            
            return new SqlConnection(connection);
        }
        public void GetEditBug(Guid id)
        {
            _bug = new Bugs();
            using SqlConnection conn = GetConnection();
            conn.Open();
            //SqlCommand cmd = new SqlCommand($"select * from BugList where id = '{id}'", conn);
            SqlCommand cmd = new SqlCommand($"select BugList.id, BugList.title, BugList.text, BugList.status, BugList.added, " +
                $"BugList.closed, BugList.catid, BaseCategory.id as catidnr, BaseCategory.cat_name from BugList join BaseCategory on " +
                $"BugList.catid = BaseCategory.catid where BugList.id = '{id}'", conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    _bug = new Bugs()
                    {
                        Id = (Guid)reader[0],
                        Title = reader["title"].ToString(),
                        Text = reader["text"].ToString(),
                        Status = reader["status"].ToString(),
                        Added = Convert.ToDateTime(reader["added"].ToString()),
                        Closed = DateTime.TryParse(reader["closed"].ToString(), out DateTime dateValue) ? Convert.ToDateTime(reader[5].ToString()) : nullableDateTime,
                        CatId = reader["catid"].ToString(),
                        CatIdNr = reader["catidnr"].ToString(),
                        CatName = reader["cat_name"].ToString()
                    };
                }
            }
            conn.Close();
        }
        public static void EditTheBug(Guid id, string title, string text, string status, string CatIdNr, string oldStatus)
        {
            title ??= "";
            text ??= "";
            status = GetStatusCat()[Convert.ToInt32(status)].Text;
            bool statusSame = oldStatus == status;
            bool statusToClosed = statusSame != true && (status == "Closed");
            bool statusToOpen = statusSame != true && (status == "Open");
            using SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand catupdt = new SqlCommand($"select * from BaseCategory where id = {CatIdNr}", conn);
            using(SqlDataReader reader = catupdt.ExecuteReader())
            {
                reader.Read();
                CatIdNr = reader["catid"].ToString();
                reader.Close();
            }
            SqlCommand cmd;
            if (statusToClosed)
            {
                cmd = new SqlCommand($"update BugList set title = @Title, text = @Text, status = @Status, " +
                    $"closed = CURRENT_TIMESTAMP, catid = @Category where id = '{id}'", conn);
            }
            else if(statusToOpen)
            {
                cmd = new SqlCommand($"update BugList set title = @Title, text = @Text, status = @Status, " +
                    $"closed = NULL, catid = @Category where id = '{id}'", conn);
            }
            else
            {
                cmd = new SqlCommand($"update BugList set title = @Title, text = @Text, status = @Status, " +
                    $"catid = @Category where id = '{id}'", conn);
            }
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Title", SqlDbType.NVarChar, 50)
            {
                Value = title
            };
            param[1] = new SqlParameter("@Text", SqlDbType.NVarChar)
            {
                Value = text
            };
            param[2] = new SqlParameter("@Status", SqlDbType.NVarChar, 50)
            {
                Value = status
            };
            param[3] = new SqlParameter("@Category", SqlDbType.NVarChar)
            {
                Value = CatIdNr
            };
            cmd.Parameters.Add(param[0]);
            cmd.Parameters.Add(param[1]);
            cmd.Parameters.Add(param[2]);
            cmd.Parameters.Add(param[3]);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void SaveData(string title, string text, string status, string category)
        {
            title ??= "";
            text ??= "";
            using SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand cmd = status == "Closed" ? 
                new SqlCommand($"insert into BugList(title, text, status, added, closed, catid) VALUES (@Title, @Text, @Status, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @Category)", conn) :
                new SqlCommand($"insert into BugList(title, text, status, added, catid) VALUES (@Title, @Text, @Status, CURRENT_TIMESTAMP, @Category)", conn);
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Title", SqlDbType.NVarChar, 50)
            {
                Value = title
            };
            param[1] = new SqlParameter("@Text", SqlDbType.NVarChar)
            {
                Value = text
            };
            param[2] = new SqlParameter("@Status", SqlDbType.NVarChar, 50)
            {
                Value = status
            };
            param[3] = new SqlParameter("@Category", SqlDbType.NVarChar)
            {
                Value = category
            };
            cmd.Parameters.Add(param[0]);
            cmd.Parameters.Add(param[1]);
            cmd.Parameters.Add(param[2]);
            cmd.Parameters.Add(param[3]);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void DeleteData(Guid id)
        {
            using SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand($"delete from BugList where id = '{id}'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void CloseBug(Guid id)
        {
            using SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand($"update BugList set status = 'Closed', closed = CURRENT_TIMESTAMP where id = '{id}'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void GetBugs()
        {
            _bugs = new List<Bugs>();
            try
            {
                using SqlConnection conn = GetConnection();
                conn.Open();
                //SqlCommand cmd = new SqlCommand("select * from BugList order by added desc", conn);
                SqlCommand cmd = new SqlCommand("select BugList.id, BugList.title, BugList.text, BugList.status, " +
                    "BugList.added, BugList.closed, BugList.catid, BaseCategory.id as catidnr, BaseCategory.cat_name " +
                    "from BugList join BaseCategory on BugList.catid = BaseCategory.catid order by added desc", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _bugs.Add(new Bugs()
                        {
                            Id = (Guid)reader["id"],
                            Title = reader["title"].ToString(),
                            Text = reader["text"].ToString(),
                            Status = reader["status"].ToString(),
                            Added = Convert.ToDateTime(reader["added"].ToString()),
                            Closed = DateTime.TryParse(reader["closed"].ToString(), out DateTime dateValue) ? Convert.ToDateTime(reader[5].ToString()) : nullableDateTime,
                            CatId = reader["catid"].ToString(),
                            CatIdNr = reader["catidnr"].ToString(),
                            CatName = reader["cat_name"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            catch
            {

            }
        }
        public void GetBugs(bool all)
        {
            SqlCommand cmd;
            try
            {
                using SqlConnection conn = GetConnection();
                conn.Open();
                if(all)
                    cmd = new SqlCommand("select COUNT(*) from BugList", conn);
                else
                    cmd = new SqlCommand("select COUNT(*) from BugList where status = 'closed'", conn);
                _bugCounts = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            catch
            {
                _bugCounts = 0;
            }
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
        public static List<SelectListItem> GetStatusCat()
        {
            _statusCat = new List<SelectListItem>() { new SelectListItem { Value = "0", Text = "Open" },
                new SelectListItem { Value = "1" , Text = "Closed" }};
            return _statusCat;
        }


        // Interface from here (IBugsRepo)
        public IEnumerable<Bugs> GetAllBugs()
        {
            GetBugs();
            return _bugs;
        }
        public Bugs GetEditBugs(Guid id)
        {
            GetEditBug(id);
            return _bug;
        }
        public IEnumerable<Category> GetBaseCategory()
        {
            GetBaseCat();
            return _category;
        }
        public IEnumerable<SelectListItem> GetStatusCategory()
        {
            GetStatusCat();
            return _statusCat;
        }
        public IEnumerable<SelectListItem> GetBaseCatList()
        {
            GetStatusCat();
            return _baseCat;
        }
        public int CountBugs(bool all)
        {
            GetBugs(all);

            return _bugCounts;
        }
    }
}
