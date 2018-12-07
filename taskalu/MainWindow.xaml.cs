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

            for (int i = 0; i < 10; i++)
            {
                // TODO: 実際の値に変更
                MainViewModel.mv.Files.Add(new ListViewFile() {
                    Name = "Image.jpg",
                    Description = "JPEG イメージ です。ここには詳細な説明を書き込むようにする予定です。したがって、自動的に改行するように画面を調整する予定です。\n長い説明をここに\n表示します。",
                    Priority = "☆☆☆☆☆",
                    CreateDate = "2011-11-11 11:1" + i.ToString(),
                    Id = i.ToString() });
            }

            this.DataContext = MainViewModel.mv;
        }

        // リストのアイテムがクリックされた時
        void ListSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ListViewFile lbf = ((sender as ListBox).SelectedItem as ListViewFile);

            listbox1.Visibility = Visibility.Collapsed;
            //MessageBox.Show(lbf.CreateDate);
            textbox1.Text = lbf.Id;
            stackpanel1.Visibility = Visibility.Visible;
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
            listbox1.Visibility = Visibility.Visible;
        }

        // ツールバーの New Task ボタン。新規タスクウィンドウを開く
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
            //dlg.DocumentMargin = this.documentTextBox.Margin;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // MessageBox.Show("OK button pressed");
                SQLiteClass.ExecuteSelectTable(MainViewModel.mv);
            }
        }

        // メニューのExitクリック
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
