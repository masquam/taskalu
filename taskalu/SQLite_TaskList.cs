using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.IO;

namespace Taskalu
{
    partial class SQLiteClass
    {
        public static string selectTaskListSql = "SELECT * FROM tasklist ";

        public static string orderBy { get; set; } = "duedate";
        public static string orderByDirection { get; set; } = "ASC";
        public static string priorityOrderByDirection { get; set; } = "ASC";
        public static string nameOrderByDirection { get; set; } = "ASC";
        public static string duedateOrderByDirection { get; set; } = "ASC";

        public static string where_status { get; set; } = "Active";
        public static string searchStringName { get; set; } = "";
        public static string searchStringDescription { get; set; } = "";
        public static string searchStringMemo { get; set; } = "";
        
        public static int moreCount { get; set; }
        public static int moreSize = 10;

        public static void ExecuteInsertTable(ListViewFile lvFile)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO tasklist (name, description, memo, priority, createdate, duedate, status, workholder) VALUES (@name, @description, @memo, @priority, @createdate, @duedate, @status, @workholder)", con);
            com.Parameters.Add(sqliteParam(com, "@name", lvFile.Name));
            com.Parameters.Add(sqliteParam(com, "@description", lvFile.Description));
            com.Parameters.Add(sqliteParam(com, "@memo", lvFile.Memo));
            com.Parameters.Add(sqliteParam(com, "@priority", lvFile.Priority));
            com.Parameters.Add(sqliteParam(com, "@createdate", System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")));
            com.Parameters.Add(sqliteParam(com, "@duedate", lvFile.DueDate));
            com.Parameters.Add(sqliteParam(com, "@status", lvFile.Status));
            com.Parameters.Add(sqliteParam(com, "@workholder", lvFile.WorkHolder));

            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private static SQLiteParameter sqliteParam(SQLiteCommand com, string paramName, string field)
        {
            SQLiteParameter param = new SQLiteParameter();
            param = com.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = System.Data.DbType.String;
            param.Direction = System.Data.ParameterDirection.Input;
            param.Value = field;
            return param;
        }

        private static SQLiteParameter sqliteParamInt64(SQLiteCommand com, string paramName, Int64 field)
        {
            SQLiteParameter param = new SQLiteParameter();
            param = com.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = System.Data.DbType.Int64;
            param.Direction = System.Data.ParameterDirection.Input;
            param.Value = field;
            return param;
        }

        public static Boolean ExecuteFirstSelectTable()
        {
            string sql = "";
            if (string.IsNullOrEmpty(searchStringMemo))
            {
                sql = selectTaskListSql + addWhereClause();
            }
            else
            {
                sql = "SELECT * FROM tasklist t INNER JOIN taskmemo m"
                     + " ON m.memo LIKE @memo AND m.tasklist_id = t.id AND t.status = 'Active' "
                     + addONClause();
            }
            sql += " order by " + orderBy + " " + orderByDirection
                + " limit " + (SQLiteClass.moreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectTable(MainViewModel.mv, sql);
        }

        public static Boolean ExecuteMoreSelectTable()
        {
            string sql = "";
            if (string.IsNullOrEmpty(searchStringMemo)) {
                sql = selectTaskListSql + addWhereClause();
            }
            else
            {
                sql = "SELECT * FROM tasklist t INNER JOIN taskmemo m"
                     + " ON m.memo LIKE @memo AND m.tasklist_id = t.id AND t.status = 'Active' "
                     + addONClause();
            }
            sql += " order by " + orderBy + " " + orderByDirection
                + " limit " + (SQLiteClass.moreSize + 1).ToString()
                + " offset " + SQLiteClass.moreCount.ToString();
            return SQLiteClass.ExecuteSelectTable(MainViewModel.mv, sql);
        }

        public static string addWhereClause()
        {
            string sql = " WHERE status = '" + where_status + "'";
            if (!String.IsNullOrEmpty(searchStringName))
            {
                sql += " AND name LIKE @name";
            }
            if (!String.IsNullOrEmpty(searchStringDescription))
            {
                sql += " AND description LIKE @description";
            }
            return sql;
        }

        public static string addONClause()
        {
            string sql = " AND t.status = '" + where_status + "'";
            if (!String.IsNullOrEmpty(searchStringName))
            {
                sql += " AND t.name LIKE @name";
            }
            if (!String.IsNullOrEmpty(searchStringDescription))
            {
                sql += " AND t.description LIKE @description";
            }
            return sql;
        }


        public static Boolean ExecuteSelectTable(MainViewModel mv, string sql)
        {
            // return value true: More button visibie
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);
            if (!String.IsNullOrEmpty(searchStringName))
            {
                com.Parameters.Add(sqliteParam(com, "@name", "%" + searchStringName + "%"));
            }
            if (!String.IsNullOrEmpty(searchStringDescription))
            {
                com.Parameters.Add(sqliteParam(com, "@description", "%" + searchStringDescription + "%"));
            }
            if (!String.IsNullOrEmpty(searchStringMemo))
            {
                com.Parameters.Add(sqliteParam(com, "@memo", "%" + searchStringMemo + "%"));
            }

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();
                int resultCount = 0;
                while (sdr.Read() == true)
                {
                    resultCount++;
                    if (resultCount <= moreSize)
                    {
                        ListViewFile lvFile = new ListViewFile();
                        lvFile.Id = (Int64)sdr["id"];
                        lvFile.Name = (string)sdr["name"];
                        lvFile.Description = (string)sdr["description"];
                        lvFile.Memo = (string)sdr["memo"];
                        lvFile.Priority = (string)sdr["priority"];

                        DateTime utc = (DateTime)sdr["createdate"];
                        lvFile.CreateDate = utc.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);

                        DateTime utc2 = (DateTime)sdr["duedate"];
                        lvFile.DueDate = utc2.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);

                        lvFile.Status = (string)sdr["status"];
                        lvFile.WorkHolder = (string)sdr["workholder"];

                        mv.Files.Add(lvFile);
                    }
                    else
                    {
                        moreCount += moreSize;
                        ret = true;
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table select error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }

        public static void SetOrderBy(string orderby, string direction)
        {
            orderBy = orderby;
            orderByDirection = direction;
        }

        public static string toggleDirection(string input)
        {
            if (input == "ASC")
            {
                return "DESC";
            }
            else
            {
                return "ASC";
            }
        }

        public static Boolean ExecuteUpdateTable(ListViewFile lvFile)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE tasklist set name=@name, description=@description, priority=@priority, createdate=@createdate, duedate=@duedate, status=@status, workholder=@workholder where id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", lvFile.Id));
            com.Parameters.Add(sqliteParam(com, "@name", lvFile.Name));
            com.Parameters.Add(sqliteParam(com, "@description", lvFile.Description));
            com.Parameters.Add(sqliteParam(com, "@priority", lvFile.Priority));
            com.Parameters.Add(sqliteParam(com, "@createdate", getUTCString(lvFile.CreateDate)));
            com.Parameters.Add(sqliteParam(com, "@duedate", getUTCString(lvFile.DueDate)));
            com.Parameters.Add(sqliteParam(com, "@status", lvFile.Status));
            com.Parameters.Add(sqliteParam(com, "@workholder", lvFile.WorkHolder));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table update error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteUpdateTable_Description(Int64 id, string description)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE tasklist set description=@description where id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));
            com.Parameters.Add(sqliteParam(com, "@description", description));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table description update error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteUpdateTaskListMemo(Int64 id, string memo)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE tasklist set memo=@memo where id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));
            com.Parameters.Add(sqliteParam(com, "@memo", memo));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table memo update error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        private static string getUTCString(string localTime)
        {
            DateTime date;
            DateTime.TryParseExact(
                localTime, "G",
                System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.None,
                out date);
            return date.ToUniversalTime()
                .ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
