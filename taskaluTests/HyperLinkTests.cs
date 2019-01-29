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
    public class HyperLinkTests
    {
        [TestMethod()]
        public void CreateHyperLinkListTest()
        {
            List<HyperLinkString> result = HyperLink.CreateHyperLinkList(@"http://example.com/ c:\ \\system07\C$\");
            Debug.Assert(string.Compare(result[0].String, @"http://example.com/") == 0);
            Debug.Assert(string.Compare(result[1].String, " ") == 0);
            Debug.Assert(string.Compare(result[2].String, @"c:\") == 0);
            Debug.Assert(string.Compare(result[3].String, " ") == 0);
            Debug.Assert(string.Compare(result[4].String, @"\\system07\C$\") == 0);
        }
    }
}