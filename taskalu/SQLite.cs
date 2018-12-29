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
    class SQLiteClass
    {
        public static string dbdirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\taskalu";
        public static string dbpath = dbdirectory + "\\taskaludb.sqlite";

        public static string selectTaskListSql = "SELECT * FROM tasklist ";

        public static string orderBy { get; set; } = "duedate";
        public static string orderByDirection { get; set; } = "ASC";
        public static string priorityOrderByDirection { get; set; } = "ASC";
        public static string nameOrderByDirection { get; set; } = "ASC";
        public static string duedateOrderByDirection { get; set; } = "ASC";

        public static string where_status { get; set; } = "Active";
        public static string searchStringName { get; set; } = "";
        public static string searchStringDescription { get; set; } = "";

        public static int moreCount { get; set; }
        public static int moreSize = 10;

        /// <summary>
        /// "touch" database - directory initialize, create table, index
        /// </summary>
        public static Boolean TouchDB()
        {
            //MessageBox.Show("The folder where database flle will be created: " + dbpath);

            Directory.CreateDirectory(dbdirectory);

            if (File.Exists(dbpath))
            {
                //MessageBox.Show("'" + dbpath + "' already exists.");
                // check database scheme
                if (!CheckTable("tasklist")){
                    return false;
                }
                if (!CheckTable("tasktime")){
                    return false;
                }
            }
            else
            {
                // create database flie
                try
                {
                    SQLiteConnection.CreateFile(dbpath);
                    MessageBox.Show("database file created.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("database file create error!\n" + ex.Message);
                    return false;
                }

                if (!ExecuteCreateTable("CREATE TABLE tasklist (id INTEGER NOT NULL PRIMARY KEY, name TEXT, description TEXT, priority TEXT, createdate DATETIME, duedate DATETIME, status TEXT, workholder TEXT)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_name", "name"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_description", "description"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_priority", "priority"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_createdate", "createdate"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_duedate", "duedate"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_status", "status"))
                {
                    return false;
                }
                if (!ExecuteCreateTable("CREATE TABLE tasktime (id INTEGER NOT NULL PRIMARY KEY, tasklist_id INTEGER, start_date TEXT, end_date TEXT, duration INTEGER)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasktime", "index_tasklist_id", "tasklist_id"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasktime", "index_start_date", "start_date"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasktime", "index_end_date", "end_date"))
                {
                    return false;
                }
            }
            return true;
        }

        public static Boolean CheckTable(string tablename)
        {
            Boolean ret = false;
            Int64 count = 0;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            SQLiteCommand com = new SQLiteCommand("SELECT COUNT(*) from sqlite_master WHERE type='table' AND name='" + tablename + "'", con);
            con.Open();
            try
            {
                count = (Int64)com.ExecuteScalar();
                if (count == 1)
                {
                    ret = true;
                }
                else
                {
                    MessageBox.Show("database table '" + tablename + "' is not created. the database file below is not for taskalu or of older version.\n\n" + dbpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table check error!\n\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteCreateTable(string sql)
        {
            Boolean ret = false;
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            SQLiteCommand com = new SQLiteCommand(sql, con);
            con.Open();
            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table create error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }


        public static void ExecuteInsertTable(ListViewFile lvFile)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO tasklist (name, description, priority, createdate, duedate, status, workholder) VALUES (@name, @description, @priority, @createdate, @duedate, @status, @workholder)", con);
            com.Parameters.Add(sqliteParam(com, "@name", lvFile.Name));
            com.Parameters.Add(sqliteParam(com, "@description", lvFile.Description));
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
            string sql = selectTaskListSql + addWhereClause();
            sql += " order by " + orderBy + " " + orderByDirection
                + " limit " + (SQLiteClass.moreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectTable(MainViewModel.mv, sql);
        }

        public static Boolean ExecuteMoreSelectTable()
        {
            string sql = selectTaskListSql + addWhereClause();
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

        public static Boolean ExecuteSelectTable(MainViewModel mv, string sql)
        {
            // return value true: More button visibie
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);
            if (!String.IsNullOrEmpty(searchStringName))
            {
                com.Parameters.Add(sqliteParam(com, "@name", "%" +searchStringName + "%"));
            }
            if (!String.IsNullOrEmpty(searchStringDescription))
            {
                com.Parameters.Add(sqliteParam(com, "@description", "%" + searchStringDescription + "%"));
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
                        lvFile.Priority = (string)sdr["priority"];

                        DateTime utc = (DateTime)sdr["createdate"];
                        lvFile.CreateDate = utc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

                        DateTime utc2 = (DateTime)sdr["duedate"];
                        lvFile.DueDate = utc2.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

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

        private static string getUTCString(string localTime)
        {
            return DateTime.ParseExact(localTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
                .ToUniversalTime()
                .ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// create index
        /// </summary>
        /// <param name="tablename">table name</param>
        /// <param name="indexname">index name</param>
        /// <param name="indexfield">index field</param>
        /// <returns></returns>
        public static Boolean ExecuteCreateIndex(string tablename, string indexname, string indexfield)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();
            SQLiteCommand com = new SQLiteCommand("CREATE INDEX " + indexname + " ON " + tablename + " (" + indexfield + ")", con);
            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database craate index error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

    }
}
