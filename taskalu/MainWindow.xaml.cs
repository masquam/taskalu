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
                    Description = "JPEG イメージ です。\n長い説明をここに\n表示します。",
                    Priority = "☆☆☆☆☆",
                    CreateDate = "2011/11/11 11:1" + i.ToString(),
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
            dlg.ShowDialog();
        }

        // メニューのExitクリック
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
