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
        public static int TaskDetailsMoreCount { get; set; }
        public static int TaskDetailsMoreSize = 20;

        public static string selectTaskDetailsTaskTimeSql = "SELECT start_date, end_date, duration FROM tasktime WHERE tasklist_id = @id";
        public static TimeSpan SumTimeSpanTaskDetails = new TimeSpan(0, 0, 0);

        public static Boolean ExecuteFirstSelecttTaskDetailsTableTaskTime(string dbpath, Int64 id)
        {
            string sql = selectTaskDetailsTaskTimeSql;
            sql += " ORDER BY end_date"
                + " LIMIT " + (SQLiteClass.TaskDetailsMoreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectTaskDetailsTableTaskTime(dbpath, TaskDetailsViewModel.tdv, sql, id);
        }

        public static Boolean ExecuteMoreSelectTaskDetailsTableTaskTime(string dbpath, Int64 id)
        {
            string sql = selectTaskDetailsTaskTimeSql;
            sql += " ORDER BY end_date"
                + " LIMIT " + (SQLiteClass.TaskDetailsMoreSize + 1).ToString()
                + " OFFSET " + SQLiteClass.TaskDetailsMoreCount.ToString();
            return SQLiteClass.ExecuteSelectTaskDetailsTableTaskTime(dbpath, TaskDetailsViewModel.tdv, sql, id);
        }

        public static Boolean ExecuteSelectTaskDetailsTableTaskTime(string dbpath, TaskDetailsViewModel tdv, string sql, Int64 id)
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
                    if (resultCount <= TaskDetailsMoreSize)
                    {
                        ListTaskDetails ltd = new ListTaskDetails();

                        DateTime utc = DateCalc.SQLiteStringToDateTime((string)sdr["start_date"]);
                        ltd.StartDate = utc.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);
                        DateTime utc2 = DateCalc.SQLiteStringToDateTime((string)sdr["end_date"]);
                        ltd.EndDate = utc2.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);

                        TimeSpan ts = new TimeSpan((Int64)sdr["duration"]);
                        ltd.Duration = ts.ToString(@"hh\:mm\:ss");
                        tdv.Entries.Add(ltd);

                        SumTimeSpanTaskDetails += ts;
                    }
                    else
                    {
                        TaskDetailsMoreCount += TaskDetailsMoreSize;
                        ret = true;
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table select tasktime TaskDetails error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }
    }
}
