﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod()]
        public void AutoGenerateTaskTest()
        {
            string dbfile = "taskaludb_autogenerate1.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            var la = new ListAutoGenerate();
            la.Order = 1;
            la.Type = 0;
            la.Name = "name";
            la.Priority = "";
            la.Template = 0;
            la.Number0 = 2;
            la.Number1 = 0;
            la.Checked_date = "2019-02-02 00:00:00";
            var result = AutoGenerate.AutoGenerateTask(path, la, new DateTime(2019, 2, 2));
            Debug.Assert(result == 2 + 4 + 8);
        }

        public void TouchTestDB(string filename)
        {
            string dbfile = filename;
            string dir = Path.GetTempPath();
            string path = Path.GetTempPath() + "\\" + dbfile;
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // preperation
            }
            SQLiteClass.TouchDB(dir, path);
        }

        [TestMethod()]
        public void AutoGenerateTaskTest2()
        {
            string dbfile = "taskaludb_autogenerate1.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            var lt = new ListTemplate();
            lt.Order = 1;
            lt.Name = "name";
            lt.Template = "template";
            SQLiteClass.ExecuteInsertTableTemplate(path, lt);

            var la = new ListAutoGenerate();
            la.Order = 1;
            la.Type = 0;
            la.Name = "name";
            la.Priority = "";
            la.Template = 1;
            la.Number0 = 2;
            la.Number1 = 0;
            la.Checked_date = "2019-02-02 00:00:00";
            var result = AutoGenerate.AutoGenerateTask(path, la, new DateTime(2019, 2, 2));
            Debug.Assert(result == 1 + 2 + 4 + 8);
        }
    }
}