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
        public static int TaskMemoMoreCount { get; set; }
        public static int TaskMemoMoreSize = 10;

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
            return SQLiteClass.ExecuteSelectTableTaskMemo(TaskMemoViewModel.tmv, sql, id);
        }

        public static Boolean ExecuteMoreSelectTableTaskMemo(Int64 id)
        {
            string sql = selectTaskMemoSql;
            sql += " ORDER BY date DESC"
                + " LIMIT " + (SQLiteClass.TaskMemoMoreSize + 1).ToString()
                + " OFFSET " + SQLiteClass.TaskMemoMoreCount.ToString();
            return SQLiteClass.ExecuteSelectTableTaskMemo(TaskMemoViewModel.tmv, sql, id);
        }

        public static Boolean ExecuteSelectTableTaskMemo(TaskMemoViewModel tmv, string sql, Int64 id)
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
                    if (resultCount <= TaskMemoMoreSize)
                    {
                        ListTaskMemo ltm = new ListTaskMemo();

                        ltm.Memo = (string)sdr["memo"];
                        DateTime utc = DateCalc.SQLiteStringToDateTime((string)sdr["date"]);
                        ltm.Date = utc.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);
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

        public static void UpdateTaskListDescription(Int64 id, string memo)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE tasklist SET description=@memo WHERE id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));
            com.Parameters.Add(sqliteParam(com, "@memo", memo));

            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table tasklist update description error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

    }
}
