using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Taskalu
{
    class HyperLink
    {
        public static List<HyperLinkString> CreateHyperLinkList(string text)
        {
            var HLList = new List<HyperLinkString>();
            splitRegex(
                text,
                @"http(s)?://([\w-]+\.)+[\w-]+(/[A-Z0-9-.,_/?%&=]*)?",
                HLList);
            return HLList;
        }

        private static void splitRegex(string text, string regexp, List<HyperLinkString> HLList)
        {
            var regex = new Regex(regexp, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var match = regex.Match(text);
            if (match.Success)
            {
                string prestring = text.Substring(0, match.Index);
                string matchedstring = match.Value;
                string poststring = text.Substring(match.Index + match.Length);

                if (!string.IsNullOrEmpty(prestring))
                {
                    HLList.Add(new HyperLinkString(prestring, HyperLinkString.Attr.String));
                }
                HLList.Add(new HyperLinkString(matchedstring, HyperLinkString.Attr.URI));

                splitRegex(poststring, regexp, HLList);
            }
            else
            {
                if (!string.IsNullOrEmpty(text))
                {
                    HLList.Add(new HyperLinkString(text, HyperLinkString.Attr.String));
                }
            }
        }

        public static void FillHyperLinks(TextBlock tb, List<HyperLinkString> HLList)
        {
            foreach (HyperLinkString HLstring in HLList)
            {
                Hyperlink hyperlink = new Hyperlink();

                switch (HLstring.Attribute) {
                    case HyperLinkString.Attr.String:
                        {
                            Run run = new Run(HLstring.String);
                            tb.Inlines.Add(run);
                            break;
                        }
                    case HyperLinkString.Attr.URI:
                        {
                            Run run = new Run(HLstring.String);
                            Hyperlink hyperl = new Hyperlink(run);
                            try
                            {
                                hyperl.NavigateUri = new Uri(HLstring.String);
                                tb.Inlines.Add(hyperl);
                            }
                            catch (Exception)
                            {
                                tb.Inlines.Add(run);
                            }
                            break;
                        }
                    default:
                        {
                            // CRLF
                            Run run = new Run("\r\n");
                            tb.Inlines.Add(run);
                            break;
                        }
                }
            }
        }
    }
}
