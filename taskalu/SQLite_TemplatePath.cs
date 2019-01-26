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
        public static void ExecuteInsertTableTemplatePath(ListTemplatePath lt)
        {
            object obj;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO template_path (template_id, torder, path) VALUES (@template_id, @torder, @path)", con);
            com.Parameters.Add(sqliteParamInt64(com, "@template_id", lt.Template_Id));
            com.Parameters.Add(sqliteParamInt64(com, "@torder", lt.Order));
            com.Parameters.Add(sqliteParam(com, "@path", lt.Path));

            try
            {
                obj = com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template path insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public static Boolean ExecuteSelectTableTemplatePath(TemplatePathListViewModel tplv, Int64 template_id)
        {
            Boolean ret = false;

            string sql = "SELECT * from template_path WHERE template_id = @template_id ORDER BY torder ASC";

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);
            com.Parameters.Add(sqliteParamInt64(com, "@template_id", template_id));

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();

                while (sdr.Read() == true)
                {
                    ListTemplatePath lt = new ListTemplatePath();
                    lt.Id = (Int64)sdr["id"];
                    lt.Template_Id = (Int64)sdr["template_id"];
                    lt.Order = (Int64)sdr["torder"];
                    lt.Path = (string)sdr["path"];
                    tplv.Entries.Add(lt);
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template path select error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }

        public static Int64 ExecuteSelectMaxTemplatePath(Int64 template_id)
        {
            Int64 torder = 0;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("SELECT MAX(torder) FROM template WHERE template_id = @template_id", con);

            try
            {
                torder = (Int64)com.ExecuteScalar();
            }
            catch (Exception)
            {
                // if no record, return DBNull -> exception raised
                torder = 0;
            }
            finally
            {
                con.Close();
            }
            return torder;
        }

        public static Boolean ExecuteUpdateTableTemplatePath(ListTemplatePath lt)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE template set template_id=@template_id, torder=@torder, path=@path where id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", lt.Id));
            com.Parameters.Add(sqliteParamInt64(com, "@tenplate_id", lt.Template_Id));
            com.Parameters.Add(sqliteParamInt64(com, "@torder", lt.Order));
            com.Parameters.Add(sqliteParam(com, "@path", lt.Path));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template path update error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
    }
}
