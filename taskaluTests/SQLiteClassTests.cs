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
            string path = dir + "\\taskaludb.sqlite";
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

        [TestMethod()]
        public void TouchDB2Test()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb.sqlite";
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

        [TestMethod()]
        public void CheckTableTest()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb.sqlite";
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

            Debug.Assert(SQLiteClass.CheckTable(path, "tasklist"));
        }

        [TestMethod()]
        public void CheckTable2Test()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb.sqlite";
            CreateSQLiteDBFlie();

            Debug.Assert(SQLiteClass.CheckTable(path, "tasklist") == false);
        }

        [TestMethod()]
        public void ExecuteCreateTableTest()
        {
            CreateSQLiteDBFlie();
            var result = CreateTableTaskList();
            Debug.Assert(result);
        }

        public void CreateSQLiteDBFlie()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb.sqlite";
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
        }

        public Boolean CreateTableTaskList()
        {
            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb.sqlite";
            return SQLiteClass.ExecuteCreateTable(path, "CREATE TABLE tasklist (id INTEGER NOT NULL PRIMARY KEY, name TEXT, description TEXT, memo TEXT, priority TEXT, createdate DATETIME, duedate DATETIME, status TEXT, workholder TEXT)");
        }
        
        [TestMethod()]
        public void ExecuteCreateIndexTest()
        {
            CreateSQLiteDBFlie();
            CreateTableTaskList();

            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb.sqlite";
            Debug.Assert(SQLiteClass.ExecuteCreateIndex(path, "tasklist", "index_tasklist_name", "name"));
        }

        [TestMethod()]
        public void ExecuteInsertTableTest()
        {
            CreateSQLiteDBFlie();
            CreateTableTaskList();

            Debug.Assert(InsertTableTaskList() == 1);
        }

        public Int64 InsertTableTaskList()
        {
            ListViewFile lvf = new ListViewFile();
            lvf.CreateDate = new DateTime(2018, 1, 1, 9, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss"); //dummy
            lvf.Description = "description";
            lvf.DueDate = new DateTime(2019, 1, 1, 9, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss");
            lvf.Id = 0; //dummy
            lvf.Memo = "memo";
            lvf.Name = "name";
            lvf.Priority = "";
            lvf.Status = "Active";
            lvf.WorkHolder = Path.GetTempPath();

            string dir = Path.GetTempPath();
            string path = dir + "\\taskaludb.sqlite";
            return SQLiteClass.ExecuteInsertTable(path, lvf);
        }
    }
}