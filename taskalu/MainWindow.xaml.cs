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
            SQLiteClass.TouchDB();
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

        // Menu - Exit
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // list item is clicked, editpanel will be visible
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
            // TODO: 実装
            WorkHolder.Open(workHolder);
        }

        // editpanel Save button
        private void ep_save_Click(object sender, RoutedEventArgs e)
        {
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


        // editpanel Close button
        private void ep_close_Click(object sender, RoutedEventArgs e)
        {
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


    }
}
