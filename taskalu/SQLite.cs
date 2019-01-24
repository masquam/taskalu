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
        public static string dbdirectory = "";
        public static string dbpath = "";

        /// <summary>
        /// "touch" database - directory initialize, create table, index
        /// </summary>
        public static Boolean TouchDB()
        {
            try
            {
                Directory.CreateDirectory(dbdirectory);
            }
            catch (Exception)
            {
                Properties.Settings.Default["Database_Folder"] = "";
                Properties.Settings.Default.Save();
                MessageBox.Show("database folder create error!\n");
                return false;
            }

            if (File.Exists(dbpath))
            {
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
                    //MessageBox.Show("database file created.");
                }
                catch (Exception ex)
                {
                    Properties.Settings.Default["Database_Folder"] = "";
                    Properties.Settings.Default.Save();
                    MessageBox.Show("database file create error!\n" + ex.Message);
                    return false;
                }

                if (!ExecuteCreateTable("CREATE TABLE tasklist (id INTEGER NOT NULL PRIMARY KEY, name TEXT, description TEXT, memo TEXT, priority TEXT, createdate DATETIME, duedate DATETIME, status TEXT, workholder TEXT)"))
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
                if (!ExecuteCreateIndex("tasklist", "index_tasklist_memo", "memo"))
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
                if (!ExecuteCreateTable("CREATE VIRTUAL TABLE strings_fts USING fts4 (id INTEGER, type TEXT, str TEXT)"))
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
