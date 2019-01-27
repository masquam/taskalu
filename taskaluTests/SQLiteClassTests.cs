using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taskalu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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
            catch(Exception)
            {
                // preperation fail
                Assert.Fail();
            }

            var result = SQLiteClass.TouchDB(dir, path);
            Debug.Assert(result);
        }
    }
}