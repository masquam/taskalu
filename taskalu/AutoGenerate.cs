using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskalu
{
    public class AutoGenerate
    {
        public static Boolean autoGenerateTasks()
        {
            Boolean result = false;

            var aglv = new AutoGenerateListViewModel();
            SQLiteClass.ExecuteSelectTableAutoGenerate(SQLiteClass.dbpath, aglv);

            foreach (ListAutoGenerate entry in aglv.Entries)
            {
                while (true)
                {
                    DateTime theNextDate = new DateTime(2019, 1, 1);
                    switch (entry.Type)
                    {
                        case (long)ListAutoGenerate.TypeName.A_Day_Of_Every_Month:
                            theNextDate = CaluculateTheNextADayOfEveryMonth(entry);
                            break;
                        case (long)ListAutoGenerate.TypeName.A_Weekday_In_Every_Week:
                            theNextDate = CaluculateTheNextAWeekDayOfEveryWeek(entry);
                            break;
                    }
                    if (theNextDate > DateTime.Now)
                    {
                        break;
                    }
                    switch (entry.Type)
                    {
                        case (long)ListAutoGenerate.TypeName.A_Day_Of_Every_Month:
                            AutoGenerateTheNextADayOfEveryMonth(entry, theNextDate);
                            result = true;
                            break;
                        case (long)ListAutoGenerate.TypeName.A_Weekday_In_Every_Week:
                            AutoGenerateTheNextAWeekDayOfEveryWeek(entry, theNextDate);
                            result = true;
                            break;
                    }
                    entry.Checked_date = theNextDate.ToString("yyyy-MM-dd HH:mm:ss");
                    SQLiteClass.ExecuteUpdateTableAutoGenerate(SQLiteClass.dbpath, entry);
                }
            }
            return result;
        }

        public static DateTime CaluculateTheNextADayOfEveryMonth(ListAutoGenerate entry)
        {
            DateTime CheckedDate;
            DateTime.TryParseExact(
                entry.Checked_date,
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out CheckedDate);
            Int32 theDay = (int)entry.Number0;
            Int32 theMonth = CheckedDate.Month;
            Int32 theYear = CheckedDate.Year;
            if (theDay <= CheckedDate.Day)
            {
                theMonth++;
                if (theMonth > 12)
                {
                    theYear++;
                    theMonth = 1;
                }
            }
            Int32 maxDay = DateTime.DaysInMonth(theYear, theMonth);
            if (theDay > maxDay)
            {
                theDay = maxDay;
            }
            return new DateTime(theYear, theMonth, theDay);
        }

        public static DateTime CaluculateTheNextAWeekDayOfEveryWeek(ListAutoGenerate entry)
        {
            DateTime CheckedDate;
            DateTime.TryParseExact(
                entry.Checked_date,
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out CheckedDate);
            Int32 theWeekday = (int)entry.Number1;
            Int32 theDay = CheckedDate.Day;
            Int32 theMonth = CheckedDate.Month;
            Int32 theYear = CheckedDate.Year;
            Int32 plusDays = theWeekday - (int)CheckedDate.DayOfWeek;
            if (plusDays <= 0)
            {
                plusDays += 7;
            }
            return new DateTime(theYear, theMonth, theDay) + new TimeSpan(plusDays, 0, 0, 0);
        }

        public static void AutoGenerateTheNextADayOfEveryMonth(ListAutoGenerate entry, DateTime theDate)
        {
            //
        }

        public static void AutoGenerateTheNextAWeekDayOfEveryWeek(ListAutoGenerate entry, DateTime theDate)
        {
            //
        }

    }
}
