using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Taskalu
{
    class EditTimer
    {
        private static DispatcherTimer dTimer { get; set; }
        private static DateTime editTimerStartDateTime { get; set; }
        private static TimeSpan editTimerSpan { get; set; }
        private static Int64 tasklist_id { get; set; }

        /// <summary>
        /// start the DispatcherTimer, Timespan initialize
        /// </summary>
        public static void start(Int64 tlist_id)
        {
            dTimer = new DispatcherTimer();
            dTimer.Tick += new EventHandler(editTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 1, 0);
            dTimer.Start();

            // Timespan init
            editTimerStartDateTime = DateTime.UtcNow;
            editTimerSpan = new TimeSpan(0, 0, 0);

            tasklist_id = tlist_id;
        }

        /// <summary>
        /// DispatcherTimer.Tick handler
        /// 
        /// update the Timespan 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void editTimer_Tick(object sender, EventArgs e)
        {
            //
            DateTime currentDateTime = DateTime.UtcNow;
            editTimerSpan = currentDateTime - editTimerStartDateTime;

            MessageBox.Show("Tick: " + editTimerSpan.ToString());

            // Forcing the CommandManager to raise the RequerySuggested event
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// stop the DispacherTimer
        /// </summary>
        public static void stop()
        {
            dTimer.Stop();
        }

    }
}
