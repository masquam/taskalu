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
    public class SQLiteClassTests
    {
        [TestMethod()]
        public void TouchDBTest()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb1.sqlite";
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // preperation fail
                Assert.Fail();
            }

            var result = SQLiteClass.TouchDB(dir, path);
            Debug.Assert(result);
        }

        /*
        // manually only, MessageBox is used
        [TestMethod()]
        public void TouchDB2Test()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb2.sqlite";
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // preperation fail
                Assert.Fail();
            }
            SQLiteConnection.CreateFile(path);

            var result = SQLiteClass.TouchDB(dir, path);
            Debug.Assert(result == false);
        }
        */

        [TestMethod()]
        public void CheckTableTest()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb3.sqlite";
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // preperation
            }
            var result = SQLiteClass.TouchDB(dir, path);

            Debug.Assert(SQLiteClass.CheckTable(path, "tasklist"));
        }

        /*
        // manually only, MessageBox is used
        [TestMethod()]
        public void CheckTable2Test()
        {
            string path = Path.GetTempPath() + "\\taskaludb4.sqlite";
            CreateSQLiteDBFlie();

            Debug.Assert(SQLiteClass.CheckTable(path, "tasklist") == false);
        }
        */

        [TestMethod()]
        public void ExecuteCreateTableTest()
        {
            string dbfile = "taskaludb5.sqlite";
            CreateSQLiteDBFlie(dbfile);
            var result = CreateTableTaskList(dbfile);
            Debug.Assert(result);
        }

        public void CreateSQLiteDBFlie(string filename)
        {
            string path = Path.GetTempPath() + "\\" + filename;
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // preperation
            }
            SQLiteConnection.CreateFile(path);
        }

        public Boolean CreateTableTaskList(string filename)
        {
            string path = Path.GetTempPath() + "\\" + filename;
            return SQLiteClass.ExecuteCreateTable(path, "CREATE TABLE tasklist (id INTEGER NOT NULL PRIMARY KEY, name TEXT, description TEXT, memo TEXT, priority TEXT, createdate DATETIME, duedate DATETIME, status TEXT, workholder TEXT)");
        }

        [TestMethod()]
        public void ExecuteCreateIndexTest()
        {
            string dbfile = "taskaludb6.sqlite";
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);

            string path = Path.GetTempPath() + "\\" + dbfile;
            Debug.Assert(SQLiteClass.ExecuteCreateIndex(path, "tasklist", "index_tasklist_name", "name"));
        }

        [TestMethod()]
        public void ExecuteInsertTableTest()
        {
            string dbfile = "taskaludb7.sqlite";
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);

            Debug.Assert(InsertTableTaskList(dbfile, "hoge", 0) == 1);
        }

        public Int64 InsertTableTaskList(string filename, string name, Int32 minute)
        {
            ListViewFile lvf = new ListViewFile();
            lvf.CreateDate = new DateTime(2018, 1, 1, 9, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss"); //dummy
            lvf.Description = "description";
            lvf.DueDate = new DateTime(2019, 1, 1, 9, minute, 0, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss");
            lvf.Id = 0; //dummy
            lvf.Memo = "memo";
            lvf.Name = name;
            lvf.Priority = "";
            lvf.Status = "Active";
            lvf.WorkHolder = Path.GetTempPath();

            string path = Path.GetTempPath() + "\\" + filename;
            return SQLiteClass.ExecuteInsertTable(path, lvf);
        }

        [TestMethod()]
        public void ExecuteFirstSelectTableTest()
        {
            string dbfile = "taskaludb8.sqlite";
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);

            SQLiteClass.moreSize = 10;
            for (int i = 0; i <= 10; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i);
            }

            string path = Path.GetTempPath() + "\\" + dbfile;
            Debug.Assert(SQLiteClass.ExecuteFirstSelectTable(path, SQLiteClass.searchString));
        }

        [TestMethod()]
        public void ExecuteFirstSelectTableFTSTest()
        {
            string dbfile = "taskaludb9.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            SQLiteClass.moreSize = 10;
            for (int i = 1; i <= 10; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i);
                SQLiteClass.ExecuteInsertTableFTSString(path, i, "tasklist_name", Ngram.getNgramText("hoge", 2));
            }
            InsertTableTaskList(dbfile, "ogem", 11);
            SQLiteClass.ExecuteInsertTableFTSString(path, 11, "tasklist_name", Ngram.getNgramText("ogem", 2));

            Debug.Assert(SQLiteClass.ExecuteFirstSelectTable(path, "oge"));
        }

        [TestMethod()]
        public void ExecuteMoreSelectTableTest()
        {
            string dbfile = "taskaludb10.sqlite";
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);

            SQLiteClass.moreSize = 10;
            for (int i = 0; i <= 20; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i);
            }

            string path = Path.GetTempPath() + "\\" + dbfile;
            Debug.Assert(SQLiteClass.ExecuteFirstSelectTable(path, SQLiteClass.searchString));
        }

        [TestMethod()]
        public void ExecuteMoreSelectTableFTSTest()
        {
            string dbfile = "taskaludb11.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            SQLiteClass.moreSize = 10;
            for (int i = 1; i <= 20; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i);
                SQLiteClass.ExecuteInsertTableFTSString(path, i, "tasklist_name", Ngram.getNgramText("hoge", 2));
            }
            InsertTableTaskList(dbfile, "ogem", 21);
            SQLiteClass.ExecuteInsertTableFTSString(path, 21, "tasklist_name", Ngram.getNgramText("ogem", 2));

            Debug.Assert(SQLiteClass.ExecuteFirstSelectTable(path, "oge"));
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
        public void ExecuteUpdateTableTest()
        {
            string dbfile = "taskaludb12.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0);

            ListViewFile lvf = new ListViewFile();
            lvf.CreateDate = new DateTime(2018, 1, 1, 9, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss"); //dummy
            lvf.Description = "description2";
            lvf.DueDate = new DateTime(2019, 1, 1, 9, 1, 0, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss");
            lvf.Id = 0; //dummy
            lvf.Memo = "memo2";
            lvf.Name = "name2";
            lvf.Priority = "";
            lvf.Status = "Active";
            lvf.WorkHolder = Path.GetTempPath();

            Debug.Assert(SQLiteClass.ExecuteUpdateTable(path, lvf));
        }

        [TestMethod()]
        public void ExecuteUpdateTaskListMemoTest()
        {
            string dbfile = "taskaludb13.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0);

            Debug.Assert(SQLiteClass.ExecuteUpdateTaskListMemo(path, 1, "hoge"));
        }

        [TestMethod()]
        public void ExecuteInsertTableTaskMemoTest()
        {
            string dbfile = "taskaludb14.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            Debug.Assert(SQLiteClass.ExecuteInsertTableTaskMemo(path, 1, "memo"));
        }

        [TestMethod()]
        public void ExecuteFirstSelectTableTaskMemoTest()
        {
            string dbfile = "taskaludb15.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1
            SQLiteClass.TaskMemoMoreSize = 10;
            for (int i = 1; i <= 11; i++)
            {
                SQLiteClass.ExecuteInsertTableTaskMemo(path, 1, "memo");
            }

            Debug.Assert(SQLiteClass.ExecuteFirstSelectTableTaskMemo(path, 1));
        }

        [TestMethod()]
        public void ExecuteMoreSelectTableTaskMemoTest()
        {
            string dbfile = "taskaludb16.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            SQLiteClass.TaskMemoMoreSize = 10;
            for (int i = 1; i <= 21; i++)
            {
                SQLiteClass.ExecuteInsertTableTaskMemo(path, 1, "memo");
            }

            Debug.Assert(SQLiteClass.ExecuteFirstSelectTableTaskMemo(path, 1));
        }


        // only valid ini Japan
        [TestMethod()]
        public void InsertOrUpdateTaskTime1Test()
        {
            string dbfile = "taskaludb17.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            var result = SQLiteClass.InsertOrUpdateTaskTime(
                path, false, 1,
                new DateTime(2019, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 14, 59, 59, DateTimeKind.Utc));
            Debug.Assert(result == true);
            Debug.Assert(table_rows_count(path, "tasktime") == 1);
        }

        // only valid ini Japan
        [TestMethod()]
        public void InsertOrUpdateTaskTime2Test()
        {
            string dbfile = "taskaludb18.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            var result = SQLiteClass.InsertOrUpdateTaskTime(
                path, false, 1,
                new DateTime(2019, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 15, 0, 0, DateTimeKind.Utc));
            Debug.Assert(result == false);
            Debug.Assert(table_rows_count(path, "tasktime") == 2);
        }

        // only valid ini Japan
        [TestMethod()]
        public void InsertOrUpdateTaskTime3Test()
        {
            string dbfile = "taskaludb19.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            // insert a tasktime for testdata
            SQLiteClass.InsertOrUpdateTaskTime(
                path, false, 1,
                new DateTime(2019, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 0, 0, 1, DateTimeKind.Utc));

            var result = SQLiteClass.InsertOrUpdateTaskTime(
                path, true, 1,
                new DateTime(2019, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 14, 59, 59, DateTimeKind.Utc));
            Debug.Assert(result == true);
            Debug.Assert(table_rows_count(path, "tasktime") == 1);
        }

        // only valid ini Japan
        [TestMethod()]
        public void InsertOrUpdateTaskTime4Test()
        {
            string dbfile = "taskaludb20.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            // insert a tasktime for testdata
            SQLiteClass.InsertOrUpdateTaskTime(
                path, false, 1,
                new DateTime(2019, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 0, 0, 1, DateTimeKind.Utc));

            var result = SQLiteClass.InsertOrUpdateTaskTime(
                path, true, 1,
                new DateTime(2019, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 15, 0, 0, DateTimeKind.Utc));
            Debug.Assert(result == false);
            Debug.Assert(table_rows_count(path, "tasktime") == 2);
        }

        public static Int64 table_rows_count(string dbpath, string tablename)
        {
            Int64 count = 0;
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();
            SQLiteCommand com = new SQLiteCommand("SELECT count(*) FROM " + tablename, con);
            try
            {
                count = (Int64)com.ExecuteScalar();
            }
            catch (Exception)
            {
                // if no record, return DBNull -> exception raised
                count = -1;
            }
            finally
            {
                con.Close();
            }
            return count;
        }

        [TestMethod()]
        public void ExecuteSumTaskTimeTest()
        {
            string dbfile = "taskaludb21.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            SQLiteClass.InsertOrUpdateTaskTime(
                path, false, 1,
                new DateTime(2019, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 16, 0, 0, DateTimeKind.Utc));

            TimeSpan result = SQLiteClass.ExecuteSumTaskTime(path, 1);
            Debug.Assert(TimeSpan.Compare(result, new TimeSpan(16, 0, 0)) == 0);
        }

        [TestMethod()]
        public void ExecuteFirstSelecttTaskDetailsTableTaskTimeTest()
        {
            string dbfile = "taskaludb22.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            SQLiteClass.TaskDetailsMoreSize = 20;
            for (int i = 1; i <= 21; i++)
            {
                SQLiteClass.InsertOrUpdateTaskTime(
                path, false, 1,
                new DateTime(2019, 2, 1, 0, i, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 0, i + 1, 0, DateTimeKind.Utc));
            }
            Debug.Assert(SQLiteClass.ExecuteFirstSelecttTaskDetailsTableTaskTime(path, 1));
        }

        [TestMethod()]
        public void ExecuteMoreSelectTaskDetailsTableTaskTimeTest()
        {
            string dbfile = "taskaludb23.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0); // tasklist id = 1

            SQLiteClass.TaskDetailsMoreSize = 20;
            SQLiteClass.TaskDetailsMoreCount = 20;
            for (int i = 1; i <= 41; i++)
            {
                SQLiteClass.InsertOrUpdateTaskTime(
                path, false, 1,
                new DateTime(2019, 2, 1, 0, i, 0, DateTimeKind.Utc),
                new DateTime(2019, 2, 1, 0, i + 1, 0, DateTimeKind.Utc));
            }
            Debug.Assert(SQLiteClass.ExecuteMoreSelectTaskDetailsTableTaskTime(path, 1));
        }

        // valid only in Japan
        [TestMethod()]
        public void ExecuteFirstSelectTableTaskTimeTest()
        {
            string dbfile = "taskaludb24.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            SQLiteClass.TaskDetailsMoreSize = 20;
            for (int i = 1; i <= 21; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i); // tasklist id = 1
                SQLiteClass.InsertOrUpdateTaskTime(
                    path, false, i,
                    new DateTime(2019, 2, 1, 0, i, 0, DateTimeKind.Utc),
                    new DateTime(2019, 2, 1, 0, i + 1, 0, DateTimeKind.Utc));
            }
            Debug.Assert(SQLiteClass.ExecuteFirstSelectTableTaskTime(path, new DateTime(2019, 2, 1)));
        }

        // valid only in Japan
        [TestMethod()]
        public void ExecuteMoreSelectTableTaskTimeTest()
        {
            string dbfile = "taskaludb25.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            SQLiteClass.TaskDetailsMoreSize = 20;
            SQLiteClass.DateSumMoreCount = 20;
            for (int i = 1; i <= 41; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i); // tasklist id = 1
                SQLiteClass.InsertOrUpdateTaskTime(
                    path, false, i,
                    new DateTime(2019, 2, 1, 0, i, 0, DateTimeKind.Utc),
                    new DateTime(2019, 2, 1, 0, i + 1, 0, DateTimeKind.Utc));
            }
            Debug.Assert(SQLiteClass.ExecuteMoreSelectTableTaskTime(path, new DateTime(2019, 2, 1)));
        }

        [TestMethod()]
        public void ExecuteFirstSelecttDateDetailsTableTaskTimeTest()
        {
            string dbfile = "taskaludb26.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            SQLiteClass.TaskDetailsMoreSize = 20;
            for (int i = 1; i <= 21; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i); // tasklist id = 1
                SQLiteClass.InsertOrUpdateTaskTime(
                    path, false, i,
                    new DateTime(2019, 2, 1, 0, i, 0, DateTimeKind.Utc),
                    new DateTime(2019, 2, 1, 0, i + 1, 0, DateTimeKind.Utc));
            }
            Debug.Assert(SQLiteClass.ExecuteFirstSelecttDateDetailsTableTaskTime(path, new DateTime(2019, 2, 1)));
        }

        [TestMethod()]
        public void ExecuteMoreSelectDateDetailsTableTaskTimeTest()
        {
            string dbfile = "taskaludb27.sqlite";
            string path = Path.GetTempPath() + "\\" + dbfile;
            TouchTestDB(dbfile);

            SQLiteClass.TaskDetailsMoreSize = 20;
            SQLiteClass.DateSumMoreCount = 20;
            for (int i = 1; i <= 41; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i); // tasklist id = 1
                SQLiteClass.InsertOrUpdateTaskTime(
                    path, false, i,
                    new DateTime(2019, 2, 1, 0, i, 0, DateTimeKind.Utc),
                    new DateTime(2019, 2, 1, 0, i + 1, 0, DateTimeKind.Utc));
            }
            Debug.Assert(SQLiteClass.ExecuteMoreSelectDateDetailsTableTaskTime(path, new DateTime(2019, 2, 1)));
        }
    }
}