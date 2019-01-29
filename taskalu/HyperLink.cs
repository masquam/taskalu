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
    public class HyperLink
    {
        private static long runsLimit = 100;

        public static List<HyperLinkString> CreateHyperLinkList(string text)
        {
            var HLList = new List<HyperLinkString>();
            var HLList2 = new List<HyperLinkString>();
            var HLList3 = new List<HyperLinkString>();
            long runs = 0;
            splitRegex(
                text,
                @"http(s)?://([\w-]+\.)+[\w-]+(/[A-Z0-9-.,_/?%&=]*)?",
                HLList,
                runs);
            splitRegex(
                HLList,
                @"([a-zA-Z]:\\[\w\\\.]*|""[a-zA-Z]:\\[\w\.].*"")",
                HLList2,
                runs);
            splitRegex(
                HLList2,
                @"(\\\\[\w\$\\\.]*|""\\\\[\w\$\.].*"")",
                HLList3,
                runs);
            return HLList3;
        }

        private static void splitRegex(string text, string regexp, List<HyperLinkString> HLList, long runs)
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

                if (runs < runsLimit)
                {
                    runs++;
                    splitRegex(poststring, regexp, HLList, runs);
                }
                else
                {
                    HLList.Add(new HyperLinkString(poststring, HyperLinkString.Attr.String));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(text))
                {
                    HLList.Add(new HyperLinkString(text, HyperLinkString.Attr.String));
                }
            }
        }

        private static void splitRegex(List<HyperLinkString> HLListBefore, string regexp, List<HyperLinkString> HLList, long runs)
        {
            foreach(HyperLinkString HLstring in HLListBefore)
            {
                if (HLstring.Attribute == HyperLinkString.Attr.URI)
                {
                    HLList.Add(HLstring);
                }
                else if (HLstring.Attribute == HyperLinkString.Attr.String)
                {
                    runs++;
                    splitRegex(HLstring.String, regexp, HLList, runs);
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
