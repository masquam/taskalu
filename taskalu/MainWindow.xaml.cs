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
        public bool taskChanged = false;

        /// <summary>
        /// MainWindow
        /// </summary>
        public MainWindow()
        {
            MainWindowUtil.setLanguageSettings();

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

            if (!SQLiteClass.TouchDB(SQLiteClass.dbdirectory, SQLiteClass.dbpath))
            {
                MessageBox.Show("database creation error.");
                Environment.Exit(3);
            }

            RecoverWindowSize();

            AutoGenerate.autoGenerateTasks();

            this.DataContext = MainViewModel.mv;
            ExecuteFirstSelectTable();

            autogenerateTimer_start();
        }

        /// <summary>
        /// Show Welcome window
        /// </summary>
        private void ShowWelcomeWindow()
        {
            WelcomeWindow dlg = new WelcomeWindow();
            dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dlg.ShowDialog();
        }

        /// <summary>
        /// Show DB folder setting window and set settings
        /// </summary>
        /// <returns>success/fail to set</returns>
        private Boolean setDBFolderSettings()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Database_Folder))
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.Description = Properties.Resources.WC_DatabaseFolder; ;
                dlg.SelectedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MainWindowUtil.AddUpdateAppSettings("Database_Folder", dlg.SelectedPath + "\\taskalu");
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

        /// <summary>
        /// Show Work folder setting window and set settings
        /// </summary>
        /// <returns>success/fail to set</returns>
        private Boolean setWorkFolderSettings()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Work_Folder))
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.Description = Properties.Resources.WC_WorkFolder;
                dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MainWindowUtil.AddUpdateAppSettings("Work_Folder", dlg.SelectedPath + "\\taskalu");
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

        /// <summary>
        /// PowerModeChanged; manage editTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(
                            SQLiteClass.TaskTimeInserted,
                            SQLiteClass.tasklist_id,
                            SQLiteClass.editTimerStartDateTime);
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

        /// <summary>
        /// Toolbar - New Task button is clicked, show New Task window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            NewWindow dlg = new NewWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dlg.ShowDialog() == true)
            {
                if (editpanel.Visibility != Visibility.Visible)
                {
                    ExecuteFirstSelectTable();
                }
            }
        }

        /// <summary>
        /// Execute fiest query for tasklist
        /// </summary>
        private void ExecuteFirstSelectTable()
        {
            MainViewModel.mv.Files.Clear();
            SQLiteClass.moreCount = 0;

            if (SQLiteClass.ExecuteFirstSelectTable(SQLiteClass.dbpath, SQLiteClass.searchString))
            {
                MoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                MoreButton.Visibility = Visibility.Collapsed;
            }
            if (MainViewModel.mv.Files.Count == 0)
            {
                MainWindowStatusBarLabel.Content = Properties.Resources.MW_NoEntry;
            }
            else
            {
                MainWindowStatusBarLabel.Content = "";
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
            OpenDatesWindow(new DateSumWindow());
        }

        /// <summary>
        /// open Date Details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDatesWindow(new DateDetailsWindow());
        }

        /// <summary>
        /// common routine for opening Date Summary/Details
        /// </summary>
        /// <param name="dlg"></param>
        private void OpenDatesWindow(Window dlg)
        {
            bool timerOnOff = false;
            if (editpanel.Visibility == Visibility.Visible)
            {
                editTimer_stop();

                // TaskTimeInserted is false then INSERT
                // else UPDATE the tasktime table
                SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(
                    SQLiteClass.TaskTimeInserted,
                    SQLiteClass.tasklist_id,
                    SQLiteClass.editTimerStartDateTime);

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

        /// <summary>
        /// status combobox changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusBox_DropDownClosed(object sender, EventArgs e)
        {
            SQLiteClass.where_status = statusBox.Text;
            ExecuteFirstSelectTable();
        }

        /// <summary>
        /// textbox memo changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxMemo_TextChanged(object sender, TextChangedEventArgs e)
        {
            SQLiteClass.searchString = textboxMemo.Text;
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


        /// <summary>
        /// More button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SQLiteClass.ExecuteMoreSelectTable(SQLiteClass.dbpath, SQLiteClass.searchString))
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

        private void Memo_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.memoOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.memoOrderByDirection);
            SQLiteClass.SetOrderBy("memo", SQLiteClass.memoOrderByDirection);
            ExecuteFirstSelectTable();
        }
        private void MemoAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.memoOrderByDirection = "ASC";
            SQLiteClass.SetOrderBy("memo", "ASC");
            ExecuteFirstSelectTable();
        }
        private void MemoDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.memoOrderByDirection = "DESC";
            SQLiteClass.SetOrderBy("memo", "DESC");
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
                    tmpDescription,
                    ep_createdate.Text,
                    ep_duedate.Text,
                    workHolder,
                    TaskMemoViewModel.tmv);
            }
            else
            {
                ClipBrd.CopyMvToClipBoard(MainViewModel.mv);
            }

        }

        /// <summary>
        ///  Menu - Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                dueDateColorConvert();
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

                taskChanged = false;
            }
        }

        /// <summary>
        /// Change forecolor of due date in editpanel
        /// </summary>
        private void dueDateColorConvert()
        {
            string datestring = ep_duedate.Text;

            DateTime date;
            DateTime.TryParseExact(datestring,
                "G",
                System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.None,
                out date);

            if (DateTime.Compare(DateTime.Now, date) > 0)
            {
                ep_duedate.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }
            else if (DateTime.Compare(DateTime.Today, date.Date) == 0)
            {
                ep_duedate.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            }
            else
            {
                ep_duedate.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            }
        }

        /// <summary>
        /// editpanel change due date button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ep_changeduedate_Click(object sender, RoutedEventArgs e)
        {
            DueDateWindow dlg = new DueDateWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlg.dueDateString = ep_duedate.Text;
            if (dlg.ShowDialog() == true)
            {
                ep_duedate.Text = dlg.dueDateString;
                dueDateColorConvert();
            }
        }

        /// <summary>
        /// editpanel open work folder button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                taskChanged = true;
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
            SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(
                SQLiteClass.TaskTimeInserted,
                SQLiteClass.tasklist_id,
                SQLiteClass.editTimerStartDateTime);

            ListViewFile lbf = new ListViewFile();
            lbf.Id = epId;
            lbf.Name = ep_name.Text;
            lbf.Description = tmpDescription;
            lbf.Priority = String.Concat(Enumerable.Repeat("\u272e", 5 - ep_priorityBox.SelectedIndex));
            lbf.CreateDate = ep_createdate.Text;
            lbf.DueDate = ep_duedate.Text;
            lbf.Status = ep_statusBox.Text;
            lbf.WorkHolder = workHolder;

            if (SQLiteClass.ExecuteUpdateTable(SQLiteClass.dbpath, lbf))
            {
                SQLiteClass.ExecuteUpdateTableFTSString(SQLiteClass.dbpath, epId, "tasklist_name", Ngram.getNgramText(ep_name.Text, 2));
                SQLiteClass.ExecuteUpdateTableFTSString(SQLiteClass.dbpath, epId, "tasklist_description", Ngram.getNgramText(tmpDescription, 2));
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

            if (taskChanged)
            {
                MessageBoxResult result = MessageBox.Show(Properties.Resources.MW_Changed,
                     "taskalu", MessageBoxButton.YesNo, MessageBoxImage.Exclamation );
                if (result == MessageBoxResult.No)
                {
                    ep_save.IsEnabled = true;
                    ep_close.IsEnabled = true;
                    return;
                }
            }

            editTimer_stop();

            // TaskTimeInserted is false then INSERT
            // else UPDATE the tasktime table
            SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(
                SQLiteClass.TaskTimeInserted,
                SQLiteClass.tasklist_id,
                SQLiteClass.editTimerStartDateTime);

            ep_CloseWindow();

            ExecuteFirstSelectTable();

            ep_save.IsEnabled = true;
            ep_close.IsEnabled = true;
        }

        /// <summary>
        /// close editpanel
        /// </summary>
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
            return SQLiteClass.InsertOrUpdateTaskTime(
                SQLiteClass.dbpath, 
                TaskTimeInserted, 
                tasklist_id, 
                editTimerStartDateTime, 
                DateTime.UtcNow);
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
            dTimer.Interval = new TimeSpan(0, 0, 10);
            dTimer.Start();

            // TimeSpan init
            SQLiteClass.editTimerStartDateTime = DateTime.UtcNow;

            // for label(summary)
            editTimerStartDateTimeForLabel = SQLiteClass.editTimerStartDateTime;
            editTimerSpan = SQLiteClass.ExecuteSumTaskTime(SQLiteClass.dbpath, tlist_id);
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
            SQLiteClass.TaskTimeInserted = InsertOrUpdateTaskTime(
                SQLiteClass.TaskTimeInserted, 
                SQLiteClass.tasklist_id, 
                SQLiteClass.editTimerStartDateTime);

            // for duedate color change
            dueDateColorConvert();

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


        private static DispatcherTimer agTimer { get; set; }
        /// <summary>
        /// start the DispatcherTimer, for refresh
        /// </summary>
        /// <param name="tlist_id">tasklist id</param>
        public void autogenerateTimer_start()
        {
            agTimer = new DispatcherTimer();
            agTimer.Tick += new EventHandler(autogenerateTimer_Tick);
            agTimer.Interval = new TimeSpan(0, 1, 0);
            agTimer.Start();
        }

        private void autogenerateTimer_Tick(object sender, EventArgs e)
        {
            var result = AutoGenerate.autoGenerateTasks();

            if (result && (listview1.Visibility == Visibility.Visible))
            {
                ExecuteFirstSelectTable();
            }
        }

        public void autogenerateTimer_stop()
        {
            agTimer.Stop();
        }


        // ///////////////////////////////////////////////////////////////////////////
        //
        // Memo

        /// <summary>
        /// taskmemo in editpanel is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListTaskMemoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListTaskMemo ltm = ((sender as ListView).SelectedItem as ListTaskMemo);

            if (ltm != null)
            {
                MemoWindow dlg = new MemoWindow();

                dlg.memo = ltm.Memo;
                dlg.date = ltm.Date;

                dlg.Owner = this;
                dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (dlg.ShowDialog() == true)
                {
                    // window is closed
                }
                listviewTaskMemo.UnselectAll();
            }
        }

        /// <summary>
        /// show Memo Add window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveMemo_Click(object sender, RoutedEventArgs e)
        {
            MemoAddWindow dlg = new MemoAddWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dlg.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(dlg.memoString))
                {
                    SQLiteClass.ExecuteInsertTableTaskMemo(SQLiteClass.dbpath, epId, dlg.memoString);
                    SQLiteClass.ExecuteUpdateTaskListMemo(SQLiteClass.dbpath, epId, dlg.memoString);
                    SQLiteClass.ExecuteInsertTableFTSString(SQLiteClass.dbpath, epId, "taskmemo", dlg.memoString);
                    ExecuteFirstSelectTableTaskMemo(epId);
                }
            }
        }

        /// <summary>
        /// execute taskmemo query
        /// </summary>
        /// <param name="id"></param>
        private void ExecuteFirstSelectTableTaskMemo(Int64 id)
        {
            TaskMemoViewModel.tmv.Memos.Clear();
            SQLiteClass.TaskMemoMoreCount = 0;
            if (SQLiteClass.ExecuteFirstSelectTableTaskMemo(SQLiteClass.dbpath, id))
            {
                TaskMemoMoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                TaskMemoMoreButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// taskmemo more button clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskMemoMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SQLiteClass.ExecuteMoreSelectTableTaskMemo(SQLiteClass.dbpath, epId))
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
            string settingvalue = Properties.Settings.Default.Language_Setting;

            if (settingvalue == "ja-JP")
            {
                LanguageSettingsWindow.language = "Japanese";
            }
            else
            {
                LanguageSettingsWindow.language = "English";
            }

            if (dlg.ShowDialog() == true)
            {
                if (LanguageSettingsWindow.language == "Japanese")
                {
                    MainWindowUtil.AddUpdateAppSettings("Language_Setting","ja-JP");
                }
                else
                {
                    MainWindowUtil.AddUpdateAppSettings("Language_Setting", "en-US");
                }
            }
        }



        // ////////////////////////////////////////////////////////////////////////
        //
        // Task Details

        /// <summary>
        /// task details button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskDetails_Click(object sender, RoutedEventArgs e)
        {
            TaskDetailsWindow dlg = new TaskDetailsWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            TaskDetailsWindow.id = epId;

            if (dlg.ShowDialog() == true)
            {
                // window is closed
            }
        }


        // /////////////////////////////////////////////////////////////////////////
        //
        // Window size management

        /// <summary>
        /// save window size
        /// </summary>
        public void SaveWindowSize()
        {
            var settings = Properties.Settings.Default;
            settings.WindowMaximized = WindowState == WindowState.Maximized;
            //WindowState = WindowState.Normal; // to get size below
            settings.WindowLeft = Left;
            settings.WindowTop = Top;
            settings.WindowWidth = Width;
            settings.WindowHeight = Height;
            settings.DescriptionHeight = descriptionHeight.ActualHeight;
            settings.Save();
        }

        /// <summary>
        /// recover window size
        /// </summary>
        void RecoverWindowSize()
        {
            var settings = Properties.Settings.Default;
            if (settings.DescriptionHeight > 0 &&
                (settings.WindowTop + settings.WindowHeight) < SystemParameters.VirtualScreenHeight)
            {
                descriptionHeight.Height = new GridLength(settings.DescriptionHeight);
            }
            if (settings.WindowLeft >= 0 &&
                (settings.WindowLeft + settings.WindowWidth) < SystemParameters.VirtualScreenWidth)
            {
                Left = settings.WindowLeft;
            }
            if (settings.WindowTop >= 0 &&
                (settings.WindowTop + settings.WindowHeight) < SystemParameters.VirtualScreenHeight)
            {
                Top = settings.WindowTop;
            }
            if (settings.WindowWidth > 0 &&
                settings.WindowWidth <= SystemParameters.WorkArea.Width)
            {
                Width = settings.WindowWidth;
            }
            if (settings.WindowHeight > 0 &&
                settings.WindowHeight <= SystemParameters.WorkArea.Height)
            {
                Height = settings.WindowHeight;
            }
            if (settings.WindowMaximized)
            {
                Loaded += (o, e) => WindowState = WindowState.Maximized;
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveWindowSize();
        }

        private void priorityChanged(object sender, SelectionChangedEventArgs e)
        {
            taskChanged = true;
        }

        private void statusChanged(object sender, SelectionChangedEventArgs e)
        {
            taskChanged = true;
        }

        private void titleChanged(object sender, TextChangedEventArgs e)
        {
            taskChanged = true;
        }

        private void duedateChanged(object sender, TextChangedEventArgs e)
        {
            taskChanged = true;
        }

        private void textBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ExecuteFirstSelectTable();
            }
        }

        // ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// open Edit Template window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplateEditButton_Click(object sender, RoutedEventArgs e)
        {
            TemplateEditWindow dlg = new TemplateEditWindow();

            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //dlg.descriptionString = tmpDescription;

            if (dlg.ShowDialog() == true)
            {
                //
            }
        }

        private void AutoGenerateEditButton_Click(object sender, RoutedEventArgs e)
        {
            AutoGenerateEditWindow dlg = new AutoGenerateEditWindow();

            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //dlg.descriptionString = tmpDescription;

            if (dlg.ShowDialog() == true)
            {
                AutoGenerate.autoGenerateTasks();
                ExecuteFirstSelectTable();
            }
        }

        private void VersionInfo_Click(object sender, RoutedEventArgs e)
        {
            VersionInfoWindow dlg = new VersionInfoWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dlg.ShowDialog() == true)
            {
                //
            }
        }
    }
}
