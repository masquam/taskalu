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
            Debug.Assert(SQLiteClass.ExecuteFirstSelectTable(path));

        }

        [TestMethod()]
        public void ExecuteMoreSelectTableTest()
        {
            string dbfile = "taskaludb.sqlite.9";
            CreateSQLiteDBFlie(dbfile);
            CreateTableTaskList(dbfile);

            SQLiteClass.moreSize = 10;
            for (int i = 0; i <= 20; i++)
            {
                InsertTableTaskList(dbfile, "hoge", i);
            }

            string path = Path.GetTempPath() + "\\" + dbfile;
            Debug.Assert(SQLiteClass.ExecuteFirstSelectTable(path));
        }
    }
}