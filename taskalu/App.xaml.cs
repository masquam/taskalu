using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;


namespace Taskalu
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(false, "taskaluMutex");

            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("taskalu - already running.");
                mutex.Close();
                mutex = null;
                this.Shutdown();
            }

            // Language Setting
            string lang = ConfigurationManager.AppSettings["Language_Setting"];
            if (lang == "ja-JP")
            {
                CultureInfo.CurrentCulture = new CultureInfo("ja-JP");
                CultureInfo.CurrentUICulture = new CultureInfo("ja-JP");
            }
            else
            {
                CultureInfo.CurrentCulture = new CultureInfo("en-US");
                CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
            }
        }

        private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            SQLiteClass.InsertOrUpdateTaskTime(SQLiteClass.TaskTimeInserted, SQLiteClass.tasklist_id, SQLiteClass.editTimerStartDateTime);
        }
    }
}
