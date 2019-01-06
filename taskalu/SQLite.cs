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

        public static int DateSumMoreCount { get; set; }
        public static int DateSumMoreSize = 10;

        public static string DateSumOrderBy { get; set; } = "duration";
        public static string DateSumOrderByDirection { get; set; } = "DESC";
        public static string DateSumNameOrderByDirection { get; set; } = "ASC";
        public static string DateSumDurationOrderByDirection { get; set; } = "DESC";

        public static int DateDetailsMoreCount { get; set; }
        public static int DateDetailsMoreSize = 10;

        public static string DateDetailsOrderBy { get; set; } = "start_date";
        public static string DateDetailsOrderByDirection { get; set; } = "ASC";
        public static string DateDetailsNameOrderByDirection { get; set; } = "ASC";
        public static string DateDetailsDurationOrderByDirection { get; set; } = "DESC";

        public static int TaskMemoMoreCount { get; set; }
        public static int TaskMemoMoreSize = 10;


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
                if (!CheckTable("tasktime"))
                {
                    return false;
                }
                if (!CheckTable("taskmemo"))
                {
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
                if (!ExecuteCreateIndex("tasklist", "index_tasklist_name", "name"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_tasklist_description", "description"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_tasklist_priority", "priority"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_tasklist_createdate", "createdate"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_tasklist_duedate", "duedate"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasklist", "index_tasklist_status", "status"))
                {
                    return false;
                }
                if (!ExecuteCreateTable("CREATE TABLE tasktime (id INTEGER NOT NULL PRIMARY KEY, tasklist_id INTEGER, date TEXT, start_date TEXT, end_date TEXT, duration INTEGER)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasktime", "index_tasktime_tasklist_id", "tasklist_id"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasktime", "index_tasktime_date", "date"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasktime", "index_tasktime_start_date", "start_date"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("tasktime", "index_tasktime_end_date", "end_date"))
                {
                    return false;
                }
                if (!ExecuteCreateTable("CREATE TABLE taskmemo (id INTEGER NOT NULL PRIMARY KEY, tasklist_id INTEGER, date TEXT, memo TEXT)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("taskmemo", "index_taskmemo_tasklist_id", "tasklist_id"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex("taskmemo", "index_taskmemo_date", "date"))
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


        // ///////////////////////////////////////////////////////////////////////////
        //
        // tasktime table

        /// <summary>
        /// INSERT or UPDATE tasktime table entry
        /// </summary>
        /// <param name="TaskTimeInserted">TaskTimeInserted flag</param>
        /// <param name="tasklist_id">tasklist_id</param>
        /// <param name="start_date">start_date</param>
        /// <returns>TaskTimeInserted flag: success of insert is true</returns>
        public static Boolean InsertOrUpdateTaskTime(
            Boolean TaskTimeInserted,
            Int64 tasklist_id,
            DateTime start_date)
        {
            Boolean ret = false;

            DateTime end_date = DateTime.UtcNow;
            TimeSpan duration = end_date - start_date;

            if (TaskTimeInserted)
            {
                if (start_date.ToLocalTime().Date == end_date.ToLocalTime().Date)
                {
                    ExecuteUpdateTableTaskTime(tasklist_id, start_date, end_date, duration);
                    ret = true;
                }
                else
                {
                    DateTime tmp_start_date = start_date.ToLocalTime().Date + new TimeSpan(1, 0, 0, 0);
                    ExecuteUpdateTableTaskTime(tasklist_id, start_date, tmp_start_date, tmp_start_date - start_date);
                    start_date = tmp_start_date; // update the argument
                    InsertOrUpdateTaskTime(false, tasklist_id, start_date);
                }
            }
            else
            {
                if (start_date.ToLocalTime().Date == end_date.ToLocalTime().Date)
                {
                    ret = ExecuteInsertTableTaskTime(tasklist_id, start_date, end_date, duration);
                }
                else
                {
                    DateTime tmp_start_date = start_date.ToLocalTime().Date + new TimeSpan(1, 0, 0, 0);
                    ExecuteInsertTableTaskTime(tasklist_id, start_date, tmp_start_date, tmp_start_date - start_date);
                    start_date = tmp_start_date; // update the argument
                    InsertOrUpdateTaskTime(false, tasklist_id, start_date);
                }
            }
            return ret;
        }

        private static Boolean ExecuteInsertTableTaskTime(Int64 tasklist_id, DateTime start_date, DateTime end_date, TimeSpan duration)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO tasktime (tasklist_id, date, start_date, end_date, duration) VALUES (@tasklist_id, @date, @start_date, @end_date, @duration)", con);
            com.Parameters.Add(sqliteParamInt64(com, "@tasklist_id", tasklist_id));
            com.Parameters.Add(sqliteParam(com, "@date", start_date.ToLocalTime().Date.ToString("yyyy-MM-dd HH:mm:ss")));
            com.Parameters.Add(sqliteParam(com, "@start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
            com.Parameters.Add(sqliteParam(com, "@end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
            com.Parameters.Add(sqliteParamInt64(com, "@duration", duration.Ticks));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table tasktime insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        private static Boolean ExecuteUpdateTableTaskTime(Int64 tasklist_id, DateTime start_date, DateTime end_date, TimeSpan duration)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE tasktime SET end_date=@end_date, duration=@duration WHERE tasklist_id=@tasklist_id AND start_date=@start_date", con);
            com.Parameters.Add(sqliteParam(com, "@end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
            com.Parameters.Add(sqliteParamInt64(com, "@duration", duration.Ticks));
            com.Parameters.Add(sqliteParamInt64(com, "@tasklist_id", tasklist_id));
            com.Parameters.Add(sqliteParam(com, "@start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));

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

        public static TimeSpan ExecuteSumTaskTime(Int64 tasklist_id)
        {
            Int64 tick = 0;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("SELECT SUM(duration) FROM tasktime WHERE tasklist_id = @tasklist_id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@tasklist_id", tasklist_id));

            try
            {
                tick = (Int64)com.ExecuteScalar();
            }
            catch (Exception)
            {
                // if no record, return DBNull -> exception raised
                tick = 0;
            }
            finally
            {
                con.Close();
            }
            return new TimeSpan(tick);
        }



        // /////////////////////////////////////////////////////////////////////
        //
        // DateSum window

        public static string selectTaskTimeSql = "SELECT t.tasklist_id, l.name name, SUM(t.duration) duration FROM tasktime t, tasklist l WHERE t.tasklist_id = l.id AND t.date = @date GROUP BY t.tasklist_id";

        public static Boolean ExecuteFirstSelectTableTaskTime(DateTime dt)
        {
            string sql = selectTaskTimeSql;
            sql += " ORDER BY " + DateSumOrderBy + " " + DateSumOrderByDirection
                + " LIMIT " + (SQLiteClass.DateSumMoreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectTableTaskTime(DateSumViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteMoreSelectTableTaskTime(DateTime dt)
        {
            string sql = selectTaskTimeSql;
            sql += " ORDER BY " + DateSumOrderBy + " " + DateSumOrderByDirection
                + " LIMIT " + (SQLiteClass.DateSumMoreSize + 1).ToString()
                + " OFFSET " + SQLiteClass.DateSumMoreCount.ToString();
            return SQLiteClass.ExecuteSelectTableTaskTime(DateSumViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteSelectTableTaskTime(DateSumViewModel dsv, string sql, DateTime dt)
        {
            // return value true: More button visibie
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);
            com.Parameters.Add(sqliteParam(com, "@date", dt.Date.ToString("yyyy-MM-dd HH:mm:ss")));

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();
                int resultCount = 0;
                while (sdr.Read() == true)
                {
                    resultCount++;
                    if (resultCount <= DateSumMoreSize)
                    {
                        ListDateSum lds = new ListDateSum();

                        lds.Name = (string)sdr["name"];

                        TimeSpan ts = new TimeSpan((Int64)sdr["duration"]);
                        //lds.Duration = ts.ToString(@"h\h\o\u\r\ m\m\i\n\u\t\e\s");
                        lds.Duration = ts.ToString(@"hh\:mm");
                        dsv.Entries.Add(lds);
                    }
                    else
                    {
                        DateSumMoreCount += DateSumMoreSize;
                        ret = true;
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table select tasktime error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }

        public static void DateSumSetOrderBy(string orderby, string direction)
        {
            DateSumOrderBy = orderby;
            DateSumOrderByDirection = direction;
        }


        // /////////////////////////////////////////////////////////////////////
        //
        // DateDetails window

        public static string selectDateDetailsTaskTimeSql = "SELECT t.tasklist_id, l.name name, t.start_date start_date, t.end_date end_date, t.duration duration FROM tasktime t, tasklist l WHERE t.tasklist_id = l.id AND t.date = @date";

        public static Boolean ExecuteFirstSelecttDateDetailsTableTaskTime(DateTime dt)
        {
            string sql = selectDateDetailsTaskTimeSql;
            sql += " ORDER BY " + DateDetailsOrderBy + " " + DateDetailsOrderByDirection
                + " LIMIT " + (SQLiteClass.DateDetailsMoreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectDateDetailsTableTaskTime(DateDetailsViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteMoreSelectDateDetailsTableTaskTime(DateTime dt)
        {
            string sql = selectDateDetailsTaskTimeSql;
            sql += " ORDER BY " + DateDetailsOrderBy + " " + DateDetailsOrderByDirection
                + " LIMIT " + (SQLiteClass.DateDetailsMoreSize + 1).ToString()
                + " OFFSET " + SQLiteClass.DateDetailsMoreCount.ToString();
            return SQLiteClass.ExecuteSelectDateDetailsTableTaskTime(DateDetailsViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteSelectDateDetailsTableTaskTime(DateDetailsViewModel dsv, string sql, DateTime dt)
        {
            // return value true: More button visibie
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);
            com.Parameters.Add(sqliteParam(com, "@date", dt.Date.ToString("yyyy-MM-dd HH:mm:ss")));

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();
                int resultCount = 0;
                while (sdr.Read() == true)
                {
                    resultCount++;
                    if (resultCount <= DateSumMoreSize)
                    {
                        ListDateDetails lds = new ListDateDetails();

                        lds.Name = (string)sdr["name"];

                        //DateTime utc = (DateTime)sdr["start_date"];
                        DateTime utc = DateTime.ParseExact((string)sdr["start_date"], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        lds.StartDate = utc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                        //DateTime utc2 = (DateTime)sdr["end_date"];
                        DateTime utc2 = DateTime.ParseExact((string)sdr["end_date"], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        lds.EndDate = utc2.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

                        TimeSpan ts = new TimeSpan((Int64)sdr["duration"]);
                        //lds.Duration = ts.ToString(@"h\h\o\u\r\ m\m\i\n\u\t\e\s");
                        lds.Duration = ts.ToString(@"hh\:mm");
                        dsv.Entries.Add(lds);
                    }
                    else
                    {
                        DateDetailsMoreCount += DateDetailsMoreSize;
                        ret = true;
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table select tasktime DateDetails error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }

        public static void DateDetailsSetOrderBy(string orderby, string direction)
        {
            DateSumOrderBy = orderby;
            DateSumOrderByDirection = direction;
        }

        // ///////////////////////////////////////////////////////////////////////////
        //
        // Memo

        public static Boolean ExecuteInsertTableTaskMemo(Int64 tasklist_id, string memo)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO taskmemo (tasklist_id, date, memo) VALUES (@tasklist_id, @date, @memo)", con);
            com.Parameters.Add(sqliteParamInt64(com, "@tasklist_id", tasklist_id));
            com.Parameters.Add(sqliteParam(com, "@date", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")));
            com.Parameters.Add(sqliteParam(com, "@memo", memo));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table taskmemo insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static string selectTaskMemoSql = "SELECT tasklist_id, date, memo FROM taskmemo WHERE tasklist_id = @id";

        public static Boolean ExecuteFirstSelectTableTaskMemo(Int64 id)
        {
            string sql = selectTaskMemoSql;
            sql += " ORDER BY date DESC"
                + " LIMIT " + (SQLiteClass.TaskMemoMoreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectTableTaskTime(TaskMemoViewModel.tmv, sql, id);
        }

        public static Boolean ExecuteMoreSelectTableTaskTime(Int64 id)
        {
            string sql = selectTaskMemoSql;
            sql += " ORDER BY date DESC"
                + " LIMIT " + (SQLiteClass.TaskMemoMoreSize + 1).ToString()
                + " OFFSET " + SQLiteClass.TaskMemoMoreCount.ToString();
            return SQLiteClass.ExecuteSelectTableTaskTime(TaskMemoViewModel.tmv, sql, id);
        }

        public static Boolean ExecuteSelectTableTaskTime(TaskMemoViewModel tmv, string sql, Int64 id)
        {
            // return value true: More button visibie
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();
                int resultCount = 0;
                while (sdr.Read() == true)
                {
                    resultCount++;
                    if (resultCount <= DateSumMoreSize)
                    {
                        ListTaskMemo ltm = new ListTaskMemo();

                        ltm.Memo = (string)sdr["memo"];
                        DateTime utc = DateTime.ParseExact((string)sdr["date"], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        ltm.Date = utc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                        tmv.Memos.Add(ltm);
                    }
                    else
                    {
                        TaskMemoMoreCount += TaskMemoMoreSize;
                        ret = true;
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table select taskmemo error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }


    }
}
