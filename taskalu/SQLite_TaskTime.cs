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
        // for TasskTime Timer
        public static DateTime editTimerStartDateTime { get; set; }
        public static Int64 tasklist_id { get; set; }
        public static Boolean TaskTimeInserted { get; set; } = false;

        /// <summary>
        /// INSERT or UPDATE tasktime table entry
        /// </summary>
        /// <param name="TaskTimeInserted">TaskTimeInserted flag</param>
        /// <param name="tasklist_id">tasklist_id</param>
        /// <param name="start_date">start_date</param>
        /// <param name="end_date_UTCNow">end_date; paticurally DateTime.UTCNow</param>
        /// <returns>TaskTimeInserted flag: success of insert is true</returns>
        public static Boolean InsertOrUpdateTaskTime(
            string dbpath,
            Boolean TaskTimeInserted,
            Int64 tasklist_id,
            DateTime start_date,
            DateTime end_date_UTCNow)
        {
            Boolean ret = false;

            //DateTime end_date = DateTime.UtcNow;
            TimeSpan duration = end_date_UTCNow - start_date;

            if (TaskTimeInserted)
            {
                if (start_date.ToLocalTime().Date == end_date_UTCNow.ToLocalTime().Date)
                {
                    ExecuteUpdateTableTaskTime(dbpath, tasklist_id, start_date, end_date_UTCNow, duration);
                    ret = true;
                }
                else
                {
                    DateTime tmp_start_date = (start_date.ToLocalTime().Date + new TimeSpan(1, 0, 0, 0)).ToUniversalTime();
                    ExecuteUpdateTableTaskTime(dbpath, tasklist_id, start_date, tmp_start_date, tmp_start_date - start_date);
                    start_date = tmp_start_date; // update the argument
                    InsertOrUpdateTaskTime(dbpath, false, tasklist_id, start_date, end_date_UTCNow);
                }
            }
            else
            {
                if (start_date.ToLocalTime().Date == end_date_UTCNow.ToLocalTime().Date)
                {
                    ret = ExecuteInsertTableTaskTime(dbpath, tasklist_id, start_date, end_date_UTCNow, duration);
                }
                else
                {
                    DateTime tmp_start_date = (start_date.ToLocalTime().Date + new TimeSpan(1, 0, 0, 0)).ToUniversalTime();
                    ExecuteInsertTableTaskTime(dbpath, tasklist_id, start_date, tmp_start_date, tmp_start_date - start_date);
                    start_date = tmp_start_date; // update the argument
                    InsertOrUpdateTaskTime(dbpath, false, tasklist_id, start_date, end_date_UTCNow);
                }
            }
            return ret;
        }

        private static Boolean ExecuteInsertTableTaskTime(string dbpath, Int64 tasklist_id, DateTime start_date, DateTime end_date, TimeSpan duration)
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

        private static Boolean ExecuteUpdateTableTaskTime(string dbpath, Int64 tasklist_id, DateTime start_date, DateTime end_date, TimeSpan duration)
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

        public static TimeSpan ExecuteSumTaskTime(string dbpath, Int64 tasklist_id)
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


    }
}
