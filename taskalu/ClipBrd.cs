using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Taskalu
{
    class ClipBrd
    {
        public static void CopyMvToClipBoard()
        {
            string str = "";

            foreach (ListViewFile file in MainViewModel.mv.Files)
            {
                str += file.Priority + "\t" + file.Name + "\t\"" + file.Description + "\"\t" + file.DueDate + "\n";
            }
            if (!string.IsNullOrEmpty(str))
            {
                Clipboard.SetText(str);
            }
        }

        public static void CopyDsvToClipBoard()
        {
            string str = "";

            foreach (ListDateSum entry in DateSumViewModel.dsv.Entries)
            {
                str += entry.Name + "\t" +  entry.Duration + "\n";
            }
            if (!string.IsNullOrEmpty(str))
            {
                Clipboard.SetText(str);
            }
        }

        public static void CopyLbfToClipBoard(
            string name,
            string priority,
            string status,
            string createdate,
            string duedate,
            string description,
            string workHolder)
        {
            string str = "";
            str = name + "\t" + priority + "\t" + status + "\t" + createdate + "\t" + duedate
                 + "\t\"" + description + "\"\t" + workHolder;
            Clipboard.SetText(str);
        }

    }
}
