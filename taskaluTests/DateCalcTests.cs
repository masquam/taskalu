using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taskalu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Taskalu.Tests
{
    [TestClass()]
    public class DateCalcTests
    {
        [TestMethod()]
        public void StringToDateTest()
        {
            DateTime result = DateCalc.StringToDate("2019/02/01", new System.Globalization.CultureInfo("ja-JP"));
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 2, 1, 0, 0, 0)) == 0);
        }

        [TestMethod()]
        public void DateToStringTest()
        {
            string result = DateCalc.DateToString(new DateTime(2019, 1, 31, 15, 0, 0), new System.Globalization.CultureInfo("ja-JP"));
            Debug.Assert(string.Compare(result, "2019/01/31") == 0);
        }

        [TestMethod()]
        public void SQLiteStringToDateTimeTest()
        {
            DateTime result = DateCalc.SQLiteStringToDateTime("2019-02-01 12:34:56");
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 2, 1, 12, 34, 56)) == 0);
        }

        [TestMethod()]
        public void getUTCStringTest()
        {
            Debug.Assert(
                string.Compare(
                    DateCalc.getUTCString("2019/02/01 08:59:59", new System.Globalization.CultureInfo("ja-JP")),
                    "2019-01-31 23:59:59") == 0);
        }
    }
}