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
        public static int DateDetailsMoreCount { get; set; }
        public static int DateDetailsMoreSize = 100;

        public static string DateDetailsOrderBy { get; set; } = "start_date";
        public static string DateDetailsOrderByDirection { get; set; } = "ASC";
        public static string DateDetailsNameOrderByDirection { get; set; } = "ASC";
        public static string DateDetailsDurationOrderByDirection { get; set; } = "DESC";

        public static string selectDateDetailsTaskTimeSql = "SELECT t.tasklist_id, l.name name, t.start_date start_date, t.end_date end_date, t.duration duration FROM tasktime t, tasklist l WHERE t.tasklist_id = l.id AND t.date = @date";
        public static TimeSpan SumTimeSpanDateDetails = new TimeSpan(0, 0, 0);

        public static Boolean ExecuteFirstSelecttDateDetailsTableTaskTime(string dbpath, DateTime dt)
        {
            SumTimeSpanDateDetails = new TimeSpan(0, 0, 0);
            string sql = selectDateDetailsTaskTimeSql;
            sql += " ORDER BY " + DateDetailsOrderBy + " " + DateDetailsOrderByDirection
                + " LIMIT " + (SQLiteClass.DateDetailsMoreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectDateDetailsTableTaskTime(dbpath, DateDetailsViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteMoreSelectDateDetailsTableTaskTime(string dbpath, DateTime dt)
        {
            string sql = selectDateDetailsTaskTimeSql;
            sql += " ORDER BY " + DateDetailsOrderBy + " " + DateDetailsOrderByDirection
                + " LIMIT " + (SQLiteClass.DateDetailsMoreSize + 1).ToString()
                + " OFFSET " + SQLiteClass.DateDetailsMoreCount.ToString();
            return SQLiteClass.ExecuteSelectDateDetailsTableTaskTime(dbpath, DateDetailsViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteSelectDateDetailsTableTaskTime(string dbpath, DateDetailsViewModel dsv, string sql, DateTime dt)
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
                    if (resultCount <= DateDetailsMoreSize)
                    {
                        ListDateDetails lds = new ListDateDetails();

                        lds.Name = (string)sdr["name"];

                        DateTime utc = DateCalc.SQLiteStringToDateTime((string)sdr["start_date"]);
                        lds.StartDate = utc.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);
                        DateTime utc2 = DateCalc.SQLiteStringToDateTime((string)sdr["end_date"]);
                        lds.EndDate = utc2.ToLocalTime().ToString("G", System.Globalization.CultureInfo.CurrentCulture);

                        TimeSpan ts = new TimeSpan((Int64)sdr["duration"]);
                        lds.Duration = ts.ToString(@"hh\:mm\:ss");
                        dsv.Entries.Add(lds);

                        SumTimeSpanDateDetails += ts;
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

        /*
        public static void DateDetailsSetOrderBy(string orderby, string direction)
        {
            DateSumOrderBy = orderby;
            DateSumOrderByDirection = direction;
        }
        */
    }
}
