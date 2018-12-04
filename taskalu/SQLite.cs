using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.IO;


namespace Taskalu
{
    class SQLiteClass
    {
        // TODO: EntityFramework License
        // https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm

        public static void TouchDB()
        {
            string dbdirectory= System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\taskalu";
            string dbpath = dbdirectory + "\\taskaludb.sqlite";
            MessageBox.Show("The folder where database flle will be created: " + dbpath);

            Directory.CreateDirectory(dbdirectory);

            if (File.Exists(dbpath))
            {
                MessageBox.Show("'" + dbpath + "' already exists.");
            }
            else
            {
                try
                {
                    SQLiteConnection.CreateFile(dbpath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("database file create error!\n" + ex.Message);
                }
                MessageBox.Show("database file created.");
            }
        }
    }
}
