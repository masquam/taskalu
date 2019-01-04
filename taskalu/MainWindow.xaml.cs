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

        public MainWindow()
        {
            InitializeComponent();
            if (!SQLiteClass.TouchDB())
            {
                MessageBox.Show("database creation error.");
                Application.Current.Shutdown();
            }
            this.DataContext = MainViewModel.mv;
            ExecuteFirstSelectTable();
        }

        /*
         * TODO: ウィンドウ幅に応じて textbox の幅を変更する
         * textblock1 が　現在のコンテキストに存在しない　エラーとなる
        void MainWindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            // change width of TextBlock Description
            textblock1.width = ActualWidth - 100;
        }
        */

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

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // new task window is closed

                // MessageBox.Show("OK button pressed");
                // TODO: 詳細パネルが開いていた場合の処理
                ExecuteFirstSelectTable();
            }
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
            // Instantiate the dialog box
            DateSumWindow dlg = new DateSumWindow();

            // Configure the dialog box
            dlg.Owner = this;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // window is closed
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
                    ep_description.Text,
                    workHolder);
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
                listviewTopToolbar.Visibility = Visibility.Collapsed;
                listviewBottomToolbar.Visibility = Visibility.Collapsed;
                listview1.Visibility = Visibility.Collapsed;

                int priorityLen = lbf.Priority.Length;
                if (priorityLen > 5) priorityLen = 5;

                epId = lbf.Id;
                ep_name.Text = lbf.Name;
                ep_description.Text = lbf.Description;
                ep_priorityBox.SelectedIndex = 5 - priorityLen;
                ep_createdate.Text = lbf.CreateDate;
                ep_duedate.Text = lbf.DueDate;
                ep_statusBox.Text = lbf.Status;
                editpanel.Visibility = Visibility.Visible;
                workHolder = lbf.WorkHolder;

                editTimer_start(epId);
            }
        }

        // editpanel change due date button
        private void ep_changeduedate_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            DueDateWindow dlg = new DueDateWindow();

            // Configure the dialog box
            dlg.Owner = this;

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
        /// editpanel Save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ep_save_Click(object sender, RoutedEventArgs e)
        {
            editTimer_stop();

            // TaskTimeInserted is false then INSERT
            // else UPDATE the tasktime table
            TaskTimeInserted = InsertOrUpdateTaskTime(TaskTimeInserted, tasklist_id, editTimerStartDateTime);

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
        }
        
        /// <summary>
        /// editpanel Close button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ep_close_Click(object sender, RoutedEventArgs e)
        {
            editTimer_stop();

            // TaskTimeInserted is false then INSERT
            // else UPDATE the tasktime table
            TaskTimeInserted = InsertOrUpdateTaskTime(TaskTimeInserted, tasklist_id, editTimerStartDateTime);

            ep_CloseWindow();
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
        private static DateTime editTimerStartDateTime { get; set; }
        private static DateTime editTimerStartDateTimeForLabel { get; set; }
        private static TimeSpan editTimerSpan { get; set; }
        private static Int64 tasklist_id { get; set; }
        private static Boolean TaskTimeInserted { get; set; } = false;

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
            editTimerStartDateTime = DateTime.UtcNow;

            // for label(summary)
            editTimerStartDateTimeForLabel = editTimerStartDateTime;
            editTimerSpan = SQLiteClass.ExecuteSumTaskTime(tlist_id);
            updateEditTimerLabel(editTimerSpan);

            // for tasktime
            tasklist_id = tlist_id;
            TaskTimeInserted = false;
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
            TaskTimeInserted = InsertOrUpdateTaskTime(TaskTimeInserted, tasklist_id, editTimerStartDateTime);

            // Forcing the CommandManager to raise the RequerySuggested event
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        private void updateEditTimerLabel(TimeSpan ts)
        {
            string str;
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
            editTimerLabel.Content = str;
        }

        /// <summary>
        /// stop the DispacherTimer
        /// </summary>
        public void editTimer_stop()
        {
            dTimer.Stop();
        }


    }
}
