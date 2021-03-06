﻿using System;
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
        public static Boolean ExecuteInsertTableTemplate(string dbpath, ListTemplate lt)
        {
            Boolean ret = false;
            object obj;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("INSERT INTO template (torder, name, template) VALUES (@torder, @name, @template)", con);
            com.Parameters.Add(sqliteParamInt64(com, "@torder", lt.Order));
            com.Parameters.Add(sqliteParam(com, "@name", lt.Name));
            com.Parameters.Add(sqliteParam(com, "@template", lt.Template));

            try
            {
                obj = com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template insert error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteSelectTableTemplate(string dbpath, TemplateListViewModel tlv)
        {
            Boolean ret = false;

            string sql = "SELECT * from template ORDER BY torder ASC";

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();

                while (sdr.Read() == true)
                {
                    ListTemplate lt = new ListTemplate();
                    lt.Id = (Int64)sdr["id"];
                    lt.Order = (Int64)sdr["torder"];
                    lt.Name = (string)sdr["name"];
                    lt.Template = (string)sdr["template"];
                    tlv.Entries.Add(lt);
                }
                sdr.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template select error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }

        public static Int64 ExecuteSelectMaxTemplate(string dbpath)
        {
            Int64 torder = 0;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("SELECT MAX(torder) FROM template", con);

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

        public static Boolean ExecuteUpdateTableTemplate(string dbpath, ListTemplate lt)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("UPDATE template set torder=@torder, name=@name, template=@template where id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", lt.Id));
            com.Parameters.Add(sqliteParamInt64(com, "@torder", lt.Order));
            com.Parameters.Add(sqliteParam(com, "@name", lt.Name));
            com.Parameters.Add(sqliteParam(com, "@template", lt.Template));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template update error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Boolean ExecuteDeleteTableTemplate(string dbpath, Int64 id)
        {
            Boolean ret = false;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("DELETE FROM template WHERE id=@id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));

            try
            {
                com.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template delete error!\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public static Int64 ExecuteSelectTemplateOrderFromID(string dbpath, Int64 id)
        {
            Int64 torder = 0;

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand("SELECT torder FROM template WHERE id = @id", con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));

            try
            {
                torder = (Int64)com.ExecuteScalar();
            }
            catch (Exception)
            {
                torder = -1;
            }
            finally
            {
                con.Close();
            }
            return torder;
        }

        public static Boolean ExecuteSelectATableTemplate(string dbpath, TemplateListViewModel tlv, Int64 id)
        {
            Boolean ret = false;

            string sql = "SELECT * from template WHERE id = @id";

            SQLiteConnection con = new SQLiteConnection("Data Source=" + dbpath + ";");
            con.Open();

            SQLiteCommand com = new SQLiteCommand(sql, con);
            com.Parameters.Add(sqliteParamInt64(com, "@id", id));

            try
            {
                SQLiteDataReader sdr = com.ExecuteReader();

                while (sdr.Read() == true)
                {
                    ListTemplate lt = new ListTemplate();
                    lt.Id = (Int64)sdr["id"];
                    lt.Order = (Int64)sdr["torder"];
                    lt.Name = (string)sdr["name"];
                    lt.Template = (string)sdr["template"];
                    tlv.Entries.Add(lt);
                }
                sdr.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("database table template select error!\n" + ex.Message);
            }
            finally
            {
                con.Close();

            }
            return ret;
        }
    }
}
