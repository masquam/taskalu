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
        public MainWindow()
        {
            InitializeComponent();

            SQLiteClass.TouchDB();

            this.DataContext = MainViewModel.mv;

            ExecuteFirstSelectTable();
        }

        // リストのアイテムがクリックされた時
        void ListSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ListViewFile lbf = ((sender as ListBox).SelectedItem as ListViewFile);

            if (lbf == null)
            {
                // nop
            }
            else
            { 
                listview1.Visibility = Visibility.Collapsed;
                ep_name.Text = lbf.Name;
                ep_description.Text = lbf.Description;
                ep_priority.Text = lbf.Priority;
                ep_createdate.Text = lbf.CreateDate;
                ep_duedate.Text = lbf.DueDate;
                ep_status.Text = lbf.Status;
                editpanel.Visibility = Visibility.Visible;
            }
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

        // 明細画面の閉じるボタン
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            listview1.UnselectAll();
            editpanel.Visibility = Visibility.Collapsed;
            listview1.Visibility = Visibility.Visible;
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

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // new task window is closed

                // MessageBox.Show("OK button pressed");
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

    }
}
