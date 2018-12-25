using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Taskalu
{
    class WorkHolder
    {
        // TODO: ユーザーが指定可能にする
        public static string workDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\taskalu";

        /// <summary>
        /// タスクに対するワークフォルダを作成し、そのディレクトリ名を返す
        /// </summary>
        /// <param name="title">タスクのタイトル</param>
        /// <returns>ディレクトリ名</returns>
        public static string CreateWorkHolder(string title)
        {
            string dir = workDirectory;
            Directory.CreateDirectory(dir);
            DateTime now = DateTime.Now;
            dir += "\\" + now.ToString("yyyy");
            Directory.CreateDirectory(dir);
            dir += "\\" + now.ToString("MM");
            Directory.CreateDirectory(dir);
            dir += "\\" + now.ToString("dd");
            Directory.CreateDirectory(dir);
            dir += "\\" + now.ToString("HHmmss");
            if ((title == null) || (title.Length == 0))
            {
                // nop
            }
            else if (title.Length <= 8)
            {
                dir += "_" + title;
            }
            else {
                dir += "_" + title.Substring(0, 8);
            }
            Directory.CreateDirectory(dir);

            return dir;
        }

        /// <summary>
        /// 指定ディレクトリを開く
        /// </summary>
        /// <param name="dir"></param>
        public static void Open(string dir)
        {
            try
            {
                System.Diagnostics.Process.Start(dir);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + dir);
            }
        }
    }
}
