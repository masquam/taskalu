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
    partial class SQLiteClass
    {
        public static Boolean ExecuteInsertTableFTSString(Int64 id, string type, string str)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO strings_fts (id, type, str) VALUES (@id, @type, @str)", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));
            com.Parameters.Add(sqliteParam(com, "@type", type));
            com.Parameters.Add(sqliteParam(com, "@str", Ngram.getNgramText(str, 2)));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table strings_fts insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteUpdateTableFTSString(Int64 id, string type, string str)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE strings_fts set id=@id, type=@type, str=@str where id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));
            com.Parameters.Add(sqliteParam(com, "@type", type));
            com.Parameters.Add(sqliteParam(com, "@str", str));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table update strings_fts error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
    }
}
