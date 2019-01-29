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
        public static int DateSumMoreCount { get; set; }
        public static int DateSumMoreSize = 20;

        public static string DateSumOrderBy { get; set; } = "duration";
        public static string DateSumOrderByDirection { get; set; } = "DESC";
        public static string DateSumNameOrderByDirection { get; set; } = "ASC";
        public static string DateSumDurationOrderByDirection { get; set; } = "DESC";

        public static string selectTaskTimeSql = "SELECT t.tasklist_id, l.name name, SUM(t.duration) duration FROM tasktime t, tasklist l WHERE t.tasklist_id = l.id AND t.date = @date GROUP BY t.tasklist_id";
        public static TimeSpan SumTimeSpanDateSum = new TimeSpan(0, 0, 0);

        public static Boolean ExecuteFirstSelectTableTaskTime(string dbpath, DateTime dt)
        {
            SumTimeSpanDateSum = new TimeSpan(0, 0, 0);
            string sql = selectTaskTimeSql;
            sql += " ORDER BY " + DateSumOrderBy + " " + DateSumOrderByDirection
                + " LIMIT " + (SQLiteClass.DateSumMoreSize + 1).ToString();
            return SQLiteClass.ExecuteSelectTableTaskTime(dbpath, DateSumViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteMoreSelectTableTaskTime(string dbpath, DateTime dt)
        {
            string sql = selectTaskTimeSql;
            sql += " ORDER BY " + DateSumOrderBy + " " + DateSumOrderByDirection
                + " LIMIT " + (SQLiteClass.DateSumMoreSize + 1).ToString()
                + " OFFSET " + SQLiteClass.DateSumMoreCount.ToString();
            return SQLiteClass.ExecuteSelectTableTaskTime(dbpath, DateSumViewModel.dsv, sql, dt);
        }

        public static Boolean ExecuteSelectTableTaskTime(string dbpath, DateSumViewModel dsv, string sql, DateTime dt)
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
                        lds.Duration = ts.ToString(@"hh\:mm\:ss");
                        dsv.Entries.Add(lds);
                        SumTimeSpanDateSum += ts;
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


    }
}
