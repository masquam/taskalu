using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taskalu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;

namespace Taskalu.Tests
{
    [TestClass()]
    public class AutoGenerateTests
    {
        [TestMethod()]
        public void CaluculateTheNextADayOfEveryMonthTest()
        {
            var lt = new ListAutoGenerate();
            lt.Order = 1;
            lt.Type = 0;
            lt.Name = "name";
            lt.Priority = "";
            lt.Template = 1;
            lt.Number0 = 2;
            lt.Number1 = 0;
            lt.Checked_date = "2019-02-01 00:00:00";
            DateTime result = AutoGenerate.CaluculateTheNextADayOfEveryMonth(lt);
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 2, 2, 0, 0, 0)) == 0);

            lt.Number0 = 2;
            lt.Checked_date = "2019-02-02 00:00:00";
            result = AutoGenerate.CaluculateTheNextADayOfEveryMonth(lt);
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 3, 2, 0, 0, 0)) == 0);

            lt.Number0 = 29;
            lt.Checked_date = "2019-02-02 00:00:00";
            result = AutoGenerate.CaluculateTheNextADayOfEveryMonth(lt);
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 2, 28, 0, 0, 0)) == 0);

            lt.Number0 = 2;
            lt.Checked_date = "2019-12-02 00:00:00";
            result = AutoGenerate.CaluculateTheNextADayOfEveryMonth(lt);
            Debug.Assert(DateTime.Compare(result, new DateTime(2020, 1, 2, 0, 0, 0)) == 0);
        }

        [TestMethod()]
        public void CaluculateTheNextAWeekDayOfEveryWeekTest()
        {
            var lt = new ListAutoGenerate();
            lt.Order = 1;
            lt.Type = 1;
            lt.Name = "name";
            lt.Priority = "";
            lt.Template = 1;
            lt.Number0 = 0;
            lt.Number1 = 0;
            lt.Checked_date = "2019-02-02 00:00:00";
            DateTime result = AutoGenerate.CaluculateTheNextAWeekDayOfEveryWeek(lt);
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 2, 3, 0, 0, 0)) == 0);

            lt.Number1 = 0;
            lt.Checked_date = "2019-02-03 00:00:00";
            result = AutoGenerate.CaluculateTheNextAWeekDayOfEveryWeek(lt);
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 2, 10, 0, 0, 0)) == 0);

            lt.Number1 = 6;
            lt.Checked_date = "2019-02-02 00:00:00";
            result = AutoGenerate.CaluculateTheNextAWeekDayOfEveryWeek(lt);
            Debug.Assert(DateTime.Compare(result, new DateTime(2019, 2, 9, 0, 0, 0)) == 0);
        }
    }
}