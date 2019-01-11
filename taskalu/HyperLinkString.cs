using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskalu
{
    public class HyperLinkString
    {
        public enum Attr { String, URI, CRLF }

        public string String { get; set; }
        public Attr Attribute { get; set; }

        public HyperLinkString(string str, Attr attr)
        {
            String = str;
            Attribute = attr;
        }

    }
}
