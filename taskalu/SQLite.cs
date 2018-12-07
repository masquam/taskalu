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

                ExecuteCreateTable("create table tasklist (id TEXT, name TEXT, description TEXT, priority TEXT, createdate DATETIME)");
            }
        }

        public static void ExecuteCreateTable(string sql)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            SQLiteCommand com = new SQLiteCommand(sql, con);
            con.Open();
            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table create error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }

        }


        public static void ExecuteInsertTable(ListViewFile lvFile)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO tasklist (id, name, description, priority, createdate) VALUES (@id, @name, @description, @priority, @createdate)", con);
            com.Parameters.Add(sqliteParam(com, "@id", lvFile.Id));
            com.Parameters.Add(sqliteParam(com, "@name", lvFile.Name));
            com.Parameters.Add(sqliteParam(com, "@description", lvFile.Description));
            com.Parameters.Add(sqliteParam(com, "@priority", lvFile.Priority));
            com.Parameters.Add(sqliteParam(com, "@createdate", System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")));

            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private static SQLiteParameter sqliteParam(SQLiteCommand com, string paramName, string field)
        {
            SQLiteParameter param = new SQLiteParameter();
            param = com.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = System.Data.DbType.String;
            param.Direction = System.Data.ParameterDirection.Input;
            param.Value = field;
            return param;
        }

        public static void ExecuteSelectTable(MainViewModel mv)
        {

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("select * from tasklist", con);

            mv.Files.Clear();
            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();
                while (sdr.Read() == true)
                {
                    ListViewFile lvFile = new ListViewFile();
                    lvFile.Id = (string)sdr["id"];
                    lvFile.Name = (string)sdr["name"];
                    lvFile.Description = (string)sdr["description"];
                    lvFile.Priority = (string)sdr["priority"];

                    DateTime utc = (DateTime)sdr["createdate"];
                    lvFile.CreateDate = utc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

                    mv.Files.Add(lvFile);
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table select error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return;
        }
    }
}
