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
            Clipboard.SetText(str);
        }
    }
}
