                                    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Threading;
using System.Globalization;
using System.Configuration;

namespace Taskalu

{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        // data behind detail window
        public Int64 epId = 0;
        public string workHolder = "";

        public string tmpDescription = "";

        public MainWindow()
        {
            setLanguageSettings();

            InitializeComponent();

            if (string.IsNullOrEmpty(Properties.Settings.Default.Database_Folder))
            {
                ShowWelcomeWindow();
            }

            if (!setDBFolderSettings())
            {
                MessageBox.Show("database folder is not selected.");
                Environment.Exit(1);
            }

            if (!setWorkFolderSettings())
            {
                MessageBox.Show("work folder is not selected.");
                Environment.Exit(2);
            }

            Microsoft.Win32.SystemEvents.PowerModeChanged +=
                new Microsoft.Win32.PowerModeChangedEventHandler(PowerModeChanged);

            if (!SQLiteClass.TouchDB())
            {
                MessageBox.Show("database creation error.");
                Environment.Exit(3);
            }
            this.DataContext = MainViewModel.mv;
            ExecuteFirstSelectTable();
        }

        private void ShowWelcomeWindow()
        {
            WelcomeWindow dlg = new WelcomeWindow();
            dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dlg.ShowDialog();
        }

        private void setLanguageSettings()
        {
            string settingvalue = Properties.Settings.Default.Language_Setting;

            if (settingvalue == "ja-JP")
            {
                CultureInfo.CurrentCulture = new CultureInfo("ja-JP");
                CultureInfo.CurrentUICulture = new CultureInfo("ja-JP");
            }
            else if (settingvalue == "en-US")
            {
                CultureInfo.CurrentCulture = new CultureInfo("en-US");
                CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            }
            else
            {
                if (CultureInfo.CurrentUICulture.Name == "ja-JP")
                {
                    CultureInfo.CurrentCulture = new CultureInfo("ja-JP");
                    CultureInfo.CurrentUICulture = new CultureInfo("ja-JP");
                    AddUpdateAppSettings("Language_Setting", "ja-JP");
                }
                else
                {
                    CultureInfo.CurrentCulture = new CultureInfo("en-US");
                    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                    AddUpdateAppSettings("Language_Setting", "en-US");
                }
            }
        }

        private Boolean setDBFolderSettings()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Database_Folder))
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.Description = Properties.Resources.WC_DatabaseFolder; ;
                dlg.SelectedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AddUpdateAppSettings("Database_Folder", dlg.SelectedPath + "\\taskalu");
                    SQLiteClass.dbdirectory = dlg.SelectedPath + "\\taskalu";
                    SQLiteClass.dbpath = SQLiteClass.dbdirectory + "\\taskaludb.sqlite";
                }
                else
                {
                    return false;
                }
            }
            else
            {
                SQLiteClass.dbdirectory = Properties.Settings.Default.Database_Folder;
                SQLiteClass.dbpath = SQLiteClass.dbdirectory + "\\taskaludb.sqlite";
            }
            return true;
        }

        private Boolean setWorkFolderSettings()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Work_Folder))
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.Description = Properties.Resources.WC_WorkFolder;
                dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AddUpdateAppSettings("Work_Folder", dlg.SelectedPath + "\\taskalu");
                    WorkHolder.workDirectory = dlg.SelectedPath + "\\taskalu";
                }
                else
                {
                    return false;
                }
            }
            else
            {
                WorkHolder.workDirectory = Properties.Settings.Default.Work_Folder;
            }
            return true;
        }

        private void PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case Microsoft.Win32.PowerModes.Suspend:
                    if (editpanel.Visibility == Visibility.Visible)
                    {
                        editTimer_stop();

                        // TaskTimeInserted is false then INSERT
                        // else UPDATE the tasktime table
                        SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(SQLiteClass.TaskTimeInserted, SQLiteClass.tasklist_id, SQLiteClass.editTimerStartDateTime);
                    }
                    break;
                case Microsoft.Win32.PowerModes.Resume:
                    if (editpanel.Visibility == Visibility.Visible)
                    {
                        editTimer_start(epId);
                    }
                    break;
            }
        }

        // Toolbar - New Task button
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            OpenNewTaskWindow();
        }

        void OpenNewTaskWindow()
        {
            // Instantiate the dialog box
            NewWindow dlg = new NewWindow();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // new task window is closed
                if (editpanel.Visibility != Visibility.Visible)
                {
                    ExecuteFirstSelectTable();
                }
            }
        }

        private void ExecuteFirstSelectTable()
        {
            MainViewModel.mv.Files.Clear();
            SQLiteClass.moreCount = 0;
            if (SQLiteClass.ExecuteFirstSelectTable())
            {
                MoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                MoreButton.Visibility = Visibility.Collapsed;
            }
            refreshTimer_start();
        }
        
        /// <summary>
        /// open Date Summary window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateSumButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDateSumWindow();
        }

        void OpenDateSumWindow()
        {
            OpenDatesWindow(new DateSumWindow());
        }

        /// <summary>
        /// open Date Details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDateDetailsWindow();
        }

        void OpenDateDetailsWindow()
        {
            OpenDatesWindow(new DateDetailsWindow());

        }

        void OpenDatesWindow(Window dlg)
        {
            bool timerOnOff = false;
            if (editpanel.Visibility == Visibility.Visible)
            {
                editTimer_stop();

                // TaskTimeInserted is false then INSERT
                // else UPDATE the tasktime table
                SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(SQLiteClass.TaskTimeInserted, SQLiteClass.tasklist_id, SQLiteClass.editTimerStartDateTime);

                timerOnOff = true;
            }

            // Configure the dialog box
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // window is closed
            }

            if (timerOnOff)
            {
                editTimer_start(epId);
            }
        }

        // status combobox is changed
        private void statusBox_DropDownClosed(object sender, EventArgs e)
        {
            SQLiteClass.where_status = statusBox.Text;
            ExecuteFirstSelectTable();
        }

        /// <summary>
        /// textbox title changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            SQLiteClass.searchStringName = textboxTitle.Text;
        }

        /// <summary>
        /// textbox description changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            SQLiteClass.searchStringDescription = textboxDescription.Text;
        }

        /// <summary>
        /// textbox memo changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxMemo_TextChanged(object sender, TextChangedEventArgs e)
        {
            SQLiteClass.searchStringMemo = textboxMemo.Text;
        }

        /// <summary>
        /// search button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            ExecuteFirstSelectTable();
        }

        // More button
        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SQLiteClass.ExecuteMoreSelectTable())
            {
                MoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                MoreButton.Visibility = Visibility.Collapsed;
            }
        }

        // ASC/DESC
        private void Priority_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.priorityOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.priorityOrderByDirection);
            SQLiteClass.SetOrderBy("priority", SQLiteClass.priorityOrderByDirection);
            ExecuteFirstSelectTable();
        }
        private void PriorityAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.priorityOrderByDirection = "ASC";
            SQLiteClass.SetOrderBy("priority", "ASC");
            ExecuteFirstSelectTable();
        }
        private void PriorityDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.priorityOrderByDirection = "DESC";
            SQLiteClass.SetOrderBy("priority", "DESC");
            ExecuteFirstSelectTable();
        }

        private void Name_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.nameOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.nameOrderByDirection);
            SQLiteClass.SetOrderBy("name", SQLiteClass.nameOrderByDirection);
            ExecuteFirstSelectTable();
        }
        private void NameAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.nameOrderByDirection = "ASC";
            SQLiteClass.SetOrderBy("name", "ASC");
            ExecuteFirstSelectTable();
        }
        private void NameDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.nameOrderByDirection = "DESC";
            SQLiteClass.SetOrderBy("name", "DESC");
            ExecuteFirstSelectTable();
        }

        private void Description_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.nameOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.nameOrderByDirection);
            SQLiteClass.SetOrderBy("description", SQLiteClass.nameOrderByDirection);
            ExecuteFirstSelectTable();
        }
        private void DescriptionAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.nameOrderByDirection = "ASC";
            SQLiteClass.SetOrderBy("description", "ASC");
            ExecuteFirstSelectTable();
        }
        private void DescriptionDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.nameOrderByDirection = "DESC";
            SQLiteClass.SetOrderBy("description", "DESC");
            ExecuteFirstSelectTable();
        }

        private void DueDate_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.duedateOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.duedateOrderByDirection);
            SQLiteClass.SetOrderBy("duedate", SQLiteClass.duedateOrderByDirection);
            ExecuteFirstSelectTable();
        }
        private void DueDateAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.duedateOrderByDirection = "ASC";
            SQLiteClass.SetOrderBy("duedate", "ASC");
            ExecuteFirstSelectTable();
        }
        private void DueDateDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.duedateOrderByDirection = "DESC";
            SQLiteClass.SetOrderBy("duedate", "DESC");
            ExecuteFirstSelectTable();
        }

        /// <summary>
        /// Copy button, to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (editpanel.Visibility == Visibility.Visible)
            {
                ClipBrd.CopyLbfToClipBoard(
                    ep_name.Text,
                    ep_priorityBox.Text,
                    ep_statusBox.Text,
                    ep_createdate.Text,
                    ep_duedate.Text,
                    workHolder,
                    TaskMemoViewModel.tmv);
            }
            else
            {
                ClipBrd.CopyMvToClipBoard();
            }

        }

        // Menu - Exit
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// list item is clicked, editpanel will be visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void ListSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ListViewFile lbf = ((sender as ListBox).SelectedItem as ListViewFile);

            if (lbf == null)
            {
                // nop
            }
            else
            {
                refreshTimer_stop();

                listviewTopToolbar.Visibility = Visibility.Collapsed;
                listviewBottomToolbar.Visibility = Visibility.Collapsed;
                listview1.Visibility = Visibility.Collapsed;

                int priorityLen = lbf.Priority.Length;
                if (priorityLen > 5) priorityLen = 5;

                epId = lbf.Id;
                ep_name.Text = lbf.Name;
                ep_description.Text = "";
                //ep_memo.Text = "";
                ep_priorityBox.SelectedIndex = 5 - priorityLen;
                ep_createdate.Text = lbf.CreateDate;
                ep_duedate.Text = lbf.DueDate;
                ep_statusBox.Text = lbf.Status;
                editpanel.Visibility = Visibility.Visible;
                workHolder = lbf.WorkHolder;

                editTimer_start(epId);

                // Memo
                listviewTaskMemo.DataContext = TaskMemoViewModel.tmv;
                ExecuteFirstSelectTableTaskMemo(epId);

                // HyperLink
                tmpDescription = lbf.Description;
                HyperLink.FillHyperLinks(ep_description, HyperLink.CreateHyperLinkList(lbf.Description));
            }
        }

        // editpanel change due date button
        private void ep_changeduedate_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            DueDateWindow dlg = new DueDateWindow();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dlg.dueDateString = ep_duedate.Text;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // due date window is closed
                ep_duedate.Text = dlg.dueDateString;
            }
        }

        // editpanel open work folder button
        private void ep_openWorkFolder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(workHolder)){
                workHolder = WorkHolder.CreateWorkHolder(ep_name.Text);
            }
            WorkHolder.Open(workHolder);
        }

        /// <summary>
        /// editpanel Edit description button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            DescriptionWindow dlg = new DescriptionWindow();

            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dlg.descriptionString = tmpDescription;

            if (dlg.ShowDialog() == true)
            {
                tmpDescription = dlg.descriptionString;
                ep_description.Text = "";
                HyperLink.FillHyperLinks(ep_description, HyperLink.CreateHyperLinkList(tmpDescription));

                SQLiteClass.ExecuteUpdateTable_Description(epId, tmpDescription);
            }
        }

        /// <summary>
        /// editpanel description URI click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestNavigateEventHandler(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.Uri.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// editpanel Save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ep_save_Click(object sender, RoutedEventArgs e)
        {
            ep_save.IsEnabled = false;
            ep_close.IsEnabled = false;

            editTimer_stop();

            // TaskTimeInserted is false then INSERT
            // else UPDATE the tasktime table
            SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(SQLiteClass.TaskTimeInserted, SQLiteClass.tasklist_id, SQLiteClass.editTimerStartDateTime);

            ListViewFile lbf = new ListViewFile();
            lbf.Id = epId;
            lbf.Name = ep_name.Text;
            lbf.Description = ep_description.Text;
            lbf.Priority = String.Concat(Enumerable.Repeat("\u272e", 5 - ep_priorityBox.SelectedIndex));
            lbf.CreateDate = ep_createdate.Text;
            lbf.DueDate = ep_duedate.Text;
            lbf.Status = ep_statusBox.Text;
            lbf.WorkHolder = workHolder;

            if (SQLiteClass.ExecuteUpdateTable(lbf))
            {
                ep_CloseWindow();
                ExecuteFirstSelectTable();
            }

            ep_save.IsEnabled = true;
            ep_close.IsEnabled = true;
        }

        /// <summary>
        /// editpanel Close button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ep_close_Click(object sender, RoutedEventArgs e)
        {
            ep_save.IsEnabled = false;
            ep_close.IsEnabled = false;

            editTimer_stop();

            // TaskTimeInserted is false then INSERT
            // else UPDATE the tasktime table
            SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(SQLiteClass.TaskTimeInserted, SQLiteClass.tasklist_id, SQLiteClass.editTimerStartDateTime);

            ep_CloseWindow();

            ExecuteFirstSelectTable();

            ep_save.IsEnabled = true;
            ep_close.IsEnabled = true;
        }

        private void ep_CloseWindow()
        {
            listview1.UnselectAll();
            editpanel.Visibility = Visibility.Collapsed;
            listviewTopToolbar.Visibility = Visibility.Visible;
            listviewBottomToolbar.Visibility = Visibility.Visible;
            listview1.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// INSERT or UPDATE tasktime table entry
        /// </summary>
        /// <param name="TaskTimeInserted">TaskTimeInserted flag</param>
        /// <param name="tasklist_id">tasklist_id</param>
        /// <param name="editTimerStartDateTime">editTimerStartDateTime</param>
        /// <returns>TaskTimeInserted flag: success of insert is true</returns>
        private Boolean InsertOrUpdateTaskTime(
            Boolean TaskTimeInserted,
            Int64 tasklist_id,
            DateTime editTimerStartDateTime)
        {
            return SQLiteClass.InsertOrUpdateTaskTime(TaskTimeInserted, tasklist_id, editTimerStartDateTime);
        }

        // ///////////////////////////////////////////////////////////////////////
        //
        // DispacherTimer

        private static DispatcherTimer dTimer { get; set; }
        //private static DateTime editTimerStartDateTime { get; set; }
        private static DateTime editTimerStartDateTimeForLabel { get; set; }
        private static TimeSpan editTimerSpan { get; set; }
        //private static Int64 tasklist_id { get; set; }
        //private static Boolean TaskTimeInserted { get; set; } = false;

        /// <summary>
        /// start the DispatcherTimer, TimeSpan initialize
        /// </summary>
        /// <param name="tlist_id">tasklist id</param>
        public void editTimer_start(Int64 tlist_id)
        {
            dTimer = new DispatcherTimer();
            dTimer.Tick += new EventHandler(editTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 1, 0);
            dTimer.Start();

            // TimeSpan init
            SQLiteClass.editTimerStartDateTime = DateTime.UtcNow;

            // for label(summary)
            editTimerStartDateTimeForLabel = SQLiteClass.editTimerStartDateTime;
            editTimerSpan = SQLiteClass.ExecuteSumTaskTime(tlist_id);
            updateEditTimerLabel(editTimerSpan);

            // for tasktime
            SQLiteClass.tasklist_id = tlist_id;
            SQLiteClass.TaskTimeInserted = false;
        }

        /// <summary>
        /// DispatcherTimer.Tick handler
        /// 
        /// update the Timespan 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editTimer_Tick(object sender, EventArgs e)
        {
            // for label(summary)
            updateEditTimerLabel(DateTime.UtcNow - editTimerStartDateTimeForLabel + editTimerSpan);

            // TaskTimeInserted is false then INSERT
            // else UPDATE the tasktime table
            // use for WHERE tasklist_id and editTimerStartDateTime
            SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(SQLiteClass.TaskTimeInserted, SQLiteClass.tasklist_id, SQLiteClass.editTimerStartDateTime);

            // Forcing the CommandManager to raise the RequerySuggested event
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        private void updateEditTimerLabel(TimeSpan ts)
        {
            string str;
            /*
            if ((ts.Days == 0) && (ts.Hours == 0)) {
                str = ts.ToString(@"m\m\i\n\u\t\e\s");
            }
            else if (ts.Days == 0)
            {
                str = ts.ToString(@"h\h\o\u\r\ m\m\i\n\u\t\e\s");
            }
            else
            {
                str = ts.ToString(@"d\d\a\y\ h\h\o\u\r\ m\m\i\n\u\t\e\s");
            }
            */
            str = ts.ToString(@"hh\:mm");
            editTimerTextBox.Text = str;
        }

        /// <summary>
        /// stop the DispacherTimer
        /// </summary>
        public void editTimer_stop()
        {
            dTimer.Stop();
        }


        private static DispatcherTimer rTimer { get; set; }
        /// <summary>
        /// start the DispatcherTimer, for refresh
        /// </summary>
        /// <param name="tlist_id">tasklist id</param>
        public void refreshTimer_start()
        {
            rTimer = new DispatcherTimer();
            rTimer.Tick += new EventHandler(refreshTimer_Tick);
            rTimer.Interval = new TimeSpan(0, 0, 10);
            rTimer.Start();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(listview1.ItemsSource);
            view.Refresh();
        }

        public void refreshTimer_stop()
        {
            rTimer.Stop();
        }

        // ///////////////////////////////////////////////////////////////////////////
        //
        // Memo

        private void ListTaskMemoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListTaskMemo ltm = ((sender as ListView).SelectedItem as ListTaskMemo);

            if (ltm != null)
            {
                // Instantiate the dialog box
                MemoWindow dlg = new MemoWindow();

                dlg.memo = ltm.Memo;
                dlg.date = ltm.Date;

                // Configure the dialog box
                dlg.Owner = this;

                dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                // Open the dialog box modally 
                if (dlg.ShowDialog() == true)
                {
                    // window is closed
                }
                listviewTaskMemo.UnselectAll();
            }
        }

        private void saveMemo_Click(object sender, RoutedEventArgs e)
        {
            MemoAddWindow dlg = new MemoAddWindow();

            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //dlg.MemoString = tmpDescription;

            if (dlg.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(dlg.memoString))
                {
                    SQLiteClass.ExecuteInsertTableTaskMemo(epId, dlg.memoString);
                    //SQLiteClass.UpdateTaskListDescription(epId, ep_memo.Text);
                    //ep_memo.Text = "";
                    ExecuteFirstSelectTableTaskMemo(epId);
                }
            }
        }

        private void ExecuteFirstSelectTableTaskMemo(Int64 id)
        {
            TaskMemoViewModel.tmv.Memos.Clear();
            SQLiteClass.TaskMemoMoreCount = 0;
            if (SQLiteClass.ExecuteFirstSelectTableTaskMemo(id))
            {
                TaskMemoMoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                TaskMemoMoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void TaskMemoMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SQLiteClass.ExecuteMoreSelectTableTaskMemo(epId))
            {
                TaskMemoMoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                TaskMemoMoreButton.Visibility = Visibility.Collapsed;
            }
        }

        // //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Language Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageSettings_Click(object sender, RoutedEventArgs e)
        {
            LanguageSettingsWindow dlg = new LanguageSettingsWindow();

            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // Language Configuration
            string settingvalue = Properties.Settings.Default.Language_Setting;

            if (settingvalue == "ja-JP")
            {
                LanguageSettingsWindow.language = "Japanese";
            }
            else
            {
                LanguageSettingsWindow.language = "English";
            }

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // window is closed OK
                //MessageBox.Show(LanguageSettingsWindow.language);

                if (LanguageSettingsWindow.language == "Japanese")
                {
                    AddUpdateAppSettings("Language_Setting","ja-JP");
                }
                else
                {
                    AddUpdateAppSettings("Language_Setting", "en-US");
                }
            }
        }
        static void AddUpdateAppSettings(string key, string value)
        {
            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
        }


        // ////////////////////////////////////////////////////////////////////////
        //
        // Task Details

        private void TaskDetails_Click(object sender, RoutedEventArgs e)
        {
            TaskDetailsWindow dlg = new TaskDetailsWindow();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            TaskDetailsWindow.id = epId;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // window is closed
            }
        }




    }

}
