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


        public static string dbdirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\taskalu";
        public static string dbpath = dbdirectory + "\\taskaludb.sqlite";

        public static void TouchDB()
        {
            //string dbdirectory= System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\taskalu";
            //string dbpath = dbdirectory + "\\taskaludb.sqlite";
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
                    MessageBox.Show("database file created.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("database file create error!\n" + ex.Message);
                }

                ExecuteCreateTable("create table tasklist (id varchar(8), name varchar(255), description varchar(255), priority varchar(255), createdate varchar(255))");
            }
        }

        public static void ExecuteCreateTable(string sql)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();
            try
            {
                SQLiteCommand com = new SQLiteCommand(sql, con);
                com.ExecuteNonQuery();
            } catch (Exception ex)
            {
                MessageBox.Show("database table create error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }


        }




    }
}
