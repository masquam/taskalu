using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskalu
{
    public class DateCalc
    {
        /// <summary>
        /// string -> date
        /// </summary>
        /// <param name="text">date string, "d" format</param>
        /// <returns>DateTime</returns>
        public static DateTime StringToDate(string text, System.Globalization.CultureInfo culture)
        {
            DateTime date;
            DateTime.TryParseExact(
                text,
                "d",
                culture,
                System.Globalization.DateTimeStyles.None,
                out date);
            return date.Date;
        }

        /// <summary>
        /// datetime -> text date only "d" format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateToString(DateTime date, System.Globalization.CultureInfo culture)
        {
            return date.Date.ToString("d", culture);
        }

        /// <summary>
        /// string -> datetime
        /// </summary>
        /// <param name="text">date string, "yyyy-MM-dd HH:mm:ss" format</param>
        /// <returns>DateTime</returns>
        public static DateTime SQLiteStringToDateTime(string text)
        {
            DateTime date;
            DateTime.TryParseExact(
                text,
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out date);
            return date;
        }

        public static string getUTCString(string localTime, System.Globalization.CultureInfo culture)
        {
            DateTime date;
            DateTime.TryParseExact(
                localTime, "G",
                culture,
                System.Globalization.DateTimeStyles.None,
                out date);
            return date.ToUniversalTime()
                .ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
