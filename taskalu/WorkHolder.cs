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
        /// create the workholder for the task, return directory string
        /// </summary>
        /// <param name="title">title of task</param>
        /// <returns>directory string</returns>
        public static string CreateWorkHolder(string title)
        {
            string dir = workDirectory;
            try
            {
                Directory.CreateDirectory(dir);
                DateTime now = DateTime.Now;
                dir += "\\" + now.ToString("yyyy");
                Directory.CreateDirectory(dir);
                dir += "\\" + now.ToString("MM");
                Directory.CreateDirectory(dir);
                dir += "\\" + now.ToString("dd");
                Directory.CreateDirectory(dir);
                dir += "\\" + now.ToString("HHmmss");

                string safeTitle = SafeFilename(title);

                if ((safeTitle == null) || (safeTitle.Length == 0))
                {
                    // nop
                }
                else if (safeTitle.Length <= 8)
                {
                    dir += "_" + safeTitle;
                }
                else
                {
                    dir += "_" + safeTitle.Substring(0, 8);
                }
                Directory.CreateDirectory(dir);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + dir);
                dir = "";
            }
            return dir;
        }

        /// <summary>
        /// Open the directory with explorer
        /// </summary>
        /// <param name="dir">directory string</param>
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

        /// <summary>
        /// remove invalid file name chars
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string SafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

    }
}
