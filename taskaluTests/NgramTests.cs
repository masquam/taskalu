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
    public class NgramTests
    {
        [TestMethod()]
        public void getNgramTextTest()
        {
            string result = Ngram.getNgramText("test", 2);
            Debug.Assert(string.Compare(result, "te es st") == 0);
        }

        [TestMethod()]
        public void getNgramTextForSearchTest()
        {
            string result = Ngram.getNgramTextForSearch("test", 2);
            Debug.Assert(string.Compare(result, "te es st") == 0);

            string result2 = Ngram.getNgramTextForSearch("t", 2);
            Debug.Assert(string.Compare(result2, "t*") == 0);
        }

        [TestMethod()]
        public void getNgramTextSpaceSeparatedTest()
        {
            string result = Ngram.getNgramTextSpaceSeparated("test hoge", 2);
            Debug.Assert(string.Compare(result, "te es st ho og ge") == 0);
        }
    }
}