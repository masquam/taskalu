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
                // TODO : 詳細画面を閉じてからすぐにlistviewの項目をクリックすると発生
                MessageBox.Show("empty task item is detected");
            }
            else
            { 
                listview1.Visibility = Visibility.Collapsed;
                textbox1.Text = lbf.Id;
                stackpanel1.Visibility = Visibility.Visible;
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
            stackpanel1.Visibility = Visibility.Collapsed;
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
            SQLiteClass.priorityOrderByDirection = toggleDirection(SQLiteClass.priorityOrderByDirection);
            SQLiteClass.orderBy = "priority";
            SQLiteClass.orderByDirection = SQLiteClass.priorityOrderByDirection;
            ExecuteFirstSelectTable();
        }
        private void PriorityAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.orderBy = "priority";
            SQLiteClass.orderByDirection = "ASC";
            SQLiteClass.priorityOrderByDirection = "ASC";
            ExecuteFirstSelectTable();
        }
        private void PriorityDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.orderBy = "priority";
            SQLiteClass.orderByDirection = "DESC";
            SQLiteClass.priorityOrderByDirection = "DESC";
            ExecuteFirstSelectTable();
        }

        private void Name_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.nameOrderByDirection = toggleDirection(SQLiteClass.nameOrderByDirection);
            SQLiteClass.orderBy = "name";
            SQLiteClass.orderByDirection = SQLiteClass.nameOrderByDirection;
            ExecuteFirstSelectTable();
        }
        private void NameAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.orderBy = "name";
            SQLiteClass.orderByDirection = "ASC";
            SQLiteClass.nameOrderByDirection = "ASC";
            ExecuteFirstSelectTable();
        }
        private void NameDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.orderBy = "name";
            SQLiteClass.orderByDirection = "DESC";
            SQLiteClass.nameOrderByDirection = "DESC";
            ExecuteFirstSelectTable();
        }

        private void DueDate_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.duedateOrderByDirection = toggleDirection(SQLiteClass.duedateOrderByDirection);
            SQLiteClass.orderBy = "duedate";
            SQLiteClass.orderByDirection = SQLiteClass.duedateOrderByDirection;
            ExecuteFirstSelectTable();
        }
        private void DueDateAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.orderBy = "duedate";
            SQLiteClass.orderByDirection = "ASC";
            SQLiteClass.duedateOrderByDirection = "ASC";
            ExecuteFirstSelectTable();
        }
        private void DueDateDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.orderBy = "duedate";
            SQLiteClass.orderByDirection = "DESC";
            SQLiteClass.duedateOrderByDirection = "DESC";
            ExecuteFirstSelectTable();
        }
        private string toggleDirection(string input)
        {
            if (input == "ASC")
            {
                return "DESC";
            }
            else
            {
                return "ASC";
            }
        }

        // Menu - Exit
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
