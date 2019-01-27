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
            string path = dir + "\\taskaludb.sqlite.1";
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
            string path = dir + "\\taskaludb.sqlite.2";
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
            string path = dir + "\\taskaludb.sqlite.3";
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
            string path = Path.GetTempPath() + "\\taskaludb.sqlite.4";
            CreateSQLiteDBFlie();

            Debug.Assert(SQLiteClass.CheckTable(path, "tasklist") == false);
        }
        */

        [TestMethod()]
        public void ExecuteCreateTableTest()
        {
            CreateSQLiteDBFlie("taskaludb.sqlite.5");
            var result = CreateTableTaskList("taskaludb.sqlite.5");
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
            CreateSQLiteDBFlie("taskaludb.sqlite.6");
            CreateTableTaskList("taskaludb.sqlite.6");

            string path = Path.GetTempPath() + "\\taskaludb.sqlite.6";
            Debug.Assert(SQLiteClass.ExecuteCreateIndex(path, "tasklist", "index_tasklist_name", "name"));
        }

        [TestMethod()]
        public void ExecuteInsertTableTest()
        {
            CreateSQLiteDBFlie("taskaludb.sqlite.7");
            CreateTableTaskList("taskaludb.sqlite.7");

            Debug.Assert(InsertTableTaskList("taskaludb.sqlite.7", "hoge", 0) == 1);
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
            string dbfile = "taskaludb.sqlite.8";
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
            string dbfile = "taskaludb.sqlite.9";
            string dir = Path.GetTempPath();
            string path = dir + "\\" + dbfile;
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // preperation
            }
            SQLiteClass.TouchDB(dir, path);

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
            string dbfile = "taskaludb.sqlite.10";
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
            string dbfile = "taskaludb.sqlite.11";
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

        [TestMethod()]
        public void ExecuteUpdateTableTest()
        {
            string dbfile = "taskaludb.sqlite.12";
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
            string dbfile = "taskaludb.sqlite.13";
            string path = Path.GetTempPath() + "\\" + dbfile;
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);
            InsertTableTaskList(dbfile, "hoge", 0);

            Debug.Assert(SQLiteClass.ExecuteUpdateTaskListMemo(path, 1, "hoge"));
        }

        [TestMethod()]
        public void getUTCStringTest()
        {
            Debug.Assert(
                string.Compare(
                    SQLiteClass.getUTCString("2019/02/01 08:59:59", new System.Globalization.CultureInfo("ja-JP")),
                    "2019-01-31 23:59:59") == 0);
        }
    }
}