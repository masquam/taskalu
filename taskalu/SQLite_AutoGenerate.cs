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
    public partial class SQLiteClass
    {
        public static Boolean ExecuteInsertTableAutoGenerate(string dbpath, ListAutoGenerate lt)
        {
            Boolean ret = false;
            object obj;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO autogenerate (torder, type, name, priority, template, number0, number1, due_hour, due_minute, checked_date) VALUES (@torder, @type, @name, @priority, @template, @number0, @number1, @due_hour, @due_minute, @checked_date)", con);
            com.Parameters.Add(sqliteParamInt64(com, "@torder", lt.Order));
            com.Parameters.Add(sqliteParamInt64(com, "@type", lt.Type));
            com.Parameters.Add(sqliteParam(com, "@name", lt.Name));
            com.Parameters.Add(sqliteParam(com, "@priority", lt.Priority));
            com.Parameters.Add(sqliteParamInt64(com, "@template", lt.Template));
            com.Parameters.Add(sqliteParamInt64(com, "@number0", lt.Number0));
            com.Parameters.Add(sqliteParamInt64(com, "@number1", lt.Number1));
            com.Parameters.Add(sqliteParamInt64(com, "@due_hour", lt.Due_hour));
            com.Parameters.Add(sqliteParamInt64(com, "@due_minute", lt.Due_minute));
            com.Parameters.Add(sqliteParam(com, "@checked_date", lt.Checked_date));

            try
            {
                obj = com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table autogenerate insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteSelectTableAutoGenerate(string dbpath, AutoGenerateListViewModel aglv)
        {
            Boolean ret = false;

            string sql = "SELECT * from autogenerate ORDER BY torder ASC";

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();

                while (sdr.Read() == true)
                {
                    ListAutoGenerate lt = new ListAutoGenerate();
                    lt.Id = (Int64)sdr["id"];
                    lt.Order = (Int64)sdr["torder"];
                    lt.Type = (Int64)sdr["type"];
                    lt.Name = (string)sdr["name"];
                    lt.Priority = (string)sdr["priority"];
                    lt.Template = (Int64)sdr["template"];
                    lt.Number0 = (Int64)sdr["number0"];                       
                    lt.Number1 = (Int64)sdr["number1"];
                    lt.Due_hour = (Int64)sdr["due_hour"];
                    lt.Due_minute = (Int64)sdr["due_minute"];
                    lt.Checked_date = (string)sdr["checked_date"];
                    aglv.Entries.Add(lt);
                }
                sdr.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table autogenerate select error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }

        public static Int64 ExecuteSelectMaxAutoGenerate(string dbpath)
        {
            Int64 torder = 0;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("SELECT MAX(torder) FROM autogenerate", con);

            try
            {
                torder = (Int64)com.ExecuteScalar();
            }
            catch (Exception)
            {
                // if no record, return DBNull -> exception raised
                torder = -1;
            }
            finally
            {
                con.Close();
            }
            return torder;
        }

        public static Boolean ExecuteUpdateTableAutoGenerate(string dbpath, ListAutoGenerate lt)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE autogenerate set torder=@torder, type=@type, name=@name, priority=@priority, template=@template, number0=@number0, number1=@number1, due_hour=@due_hour, due_minute=@due_minute, checked_date=@checked_date where id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", lt.Id));
            com.Parameters.Add(sqliteParamInt64(com, "@torder", lt.Order));
            com.Parameters.Add(sqliteParamInt64(com, "@type", lt.Type));
            com.Parameters.Add(sqliteParam(com, "@name", lt.Name));
            com.Parameters.Add(sqliteParam(com, "@priority", lt.Priority));
            com.Parameters.Add(sqliteParamInt64(com, "@template", lt.Template));
            com.Parameters.Add(sqliteParamInt64(com, "@number0", lt.Number0));
            com.Parameters.Add(sqliteParamInt64(com, "@number1", lt.Number1));
            com.Parameters.Add(sqliteParamInt64(com, "@due_hour", lt.Due_hour));
            com.Parameters.Add(sqliteParamInt64(com, "@due_minute", lt.Due_minute));
            com.Parameters.Add(sqliteParam(com, "@checked_date", lt.Checked_date));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table autogenerate update error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteDeleteTableAutoGenerate(string dbpath, Int64 id)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("DELETE FROM autogenerate WHERE id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table autogenerate delete error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Int64 ExecuteSelectTemplateUsedInAutoGenerate(string dbpath, Int64 id)
        {
            Int64 torder = 0;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("SELECT count(*) FROM autogenerate WHERE template = @id;", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));

            try
            {
                torder = (Int64)com.ExecuteScalar();
            }
            catch (Exception)
            {
                // if no record, return DBNull -> exception raised
                torder = -1;
            }
            finally
            {
                con.Close();
            }
            return torder;
        }
    }
}
