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
    public partial class SQLiteClass
    {
        public static string dbdirectory = "";
        public static string dbpath = "";

        /// <summary>
        /// "touch" database - directory initialize, create table, index
        /// </summary>
        public static Boolean TouchDB(string dbdirectory, string dbpath)
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
                if (!CheckTable(dbpath, "tasklist")){
                    return false;
                }
                if (!CheckTable(dbpath, "tasktime"))
                {
                    return false;
                }
                if (!CheckTable(dbpath, "taskmemo"))
                {
                    return false;
                }
                if (!CheckTable(dbpath, "template"))
                {
                    return false;
                }
                if (!CheckTable(dbpath, "template_path"))
                {
                    return false;
                }
                if (!CheckTable(dbpath, "autogenerate"))
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

                if (!ExecuteCreateTable(dbpath, "CREATE TABLE tasklist (id INTEGER NOT NULL PRIMARY KEY, name TEXT, description TEXT, memo TEXT, priority TEXT, createdate DATETIME, duedate DATETIME, status TEXT, workholder TEXT)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasklist", "index_tasklist_name", "name"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasklist", "index_tasklist_description", "description"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasklist", "index_tasklist_memo", "memo"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasklist", "index_tasklist_priority", "priority"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasklist", "index_tasklist_createdate", "createdate"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasklist", "index_tasklist_duedate", "duedate"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasklist", "index_tasklist_status", "status"))
                {
                    return false;
                }
                if (!ExecuteCreateTable(dbpath, "CREATE TABLE tasktime (id INTEGER NOT NULL PRIMARY KEY, tasklist_id INTEGER, date TEXT, start_date TEXT, end_date TEXT, duration INTEGER)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasktime", "index_tasktime_tasklist_id", "tasklist_id"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasktime", "index_tasktime_date", "date"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasktime", "index_tasktime_start_date", "start_date"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "tasktime", "index_tasktime_end_date", "end_date"))
                {
                    return false;
                }
                if (!ExecuteCreateTable(dbpath, "CREATE TABLE taskmemo (id INTEGER NOT NULL PRIMARY KEY, tasklist_id INTEGER, date TEXT, memo TEXT)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "taskmemo", "index_taskmemo_tasklist_id", "tasklist_id"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "taskmemo", "index_taskmemo_date", "date"))
                {
                    return false;
                }
                if (!ExecuteCreateTable(dbpath, "CREATE VIRTUAL TABLE strings_fts USING fts4 (id INTEGER, type TEXT, str TEXT)"))
                {
                    return false;
                }
                if (!ExecuteCreateTable(dbpath, "CREATE TABLE template (id INTEGER NOT NULL PRIMARY KEY, torder INTEGER, name TEXT, template TEXT)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "template", "index_template_order", "torder"))
                {
                    return false;
                }
                if (!ExecuteCreateTable(dbpath, "CREATE TABLE template_path (id INTEGER NOT NULL PRIMARY KEY, template_id INTEGER, torder INTEGER, path TEXT)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "template_path", "index_template_path_order", "torder"))
                {
                    return false;
                }
                if (!ExecuteCreateTable(dbpath, "CREATE TABLE autogenerate (id INTEGER NOT NULL PRIMARY KEY, torder INTEGER, type INTEGER, name TEXT, priority TEXT,  template INTEGER, number0 INTEGER, number1 INTEGER, due_hour INTEGER, due_minute INTEGER)"))
                {
                    return false;
                }
                if (!ExecuteCreateIndex(dbpath, "autogenerate", "index_autogenerate_order", "torder"))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// check if the table exists
        /// </summary>
        /// <param name="dbpath"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static Boolean CheckTable(string dbpath, string tablename)
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

        /// <summary>
        /// create table
        /// </summary>
        /// <param name="dbpath"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Boolean ExecuteCreateTable(string dbpath, string sql)
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
        /// <param name="dbpath"></param>
        /// <param name="tablename"></param>
        /// <param name="indexname"></param>
        /// <param name="indexfield"></param>
        /// <returns></returns>
        public static Boolean ExecuteCreateIndex(string dbpath, string tablename, string indexname, string indexfield)
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
