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

            MainViewModel mv = new MainViewModel();

            for (int i = 0; i < 10; i++)
            {
                // TODO: 実際の値に変更
                mv.Files.Add(new ListViewFile() { Name = "Image.jpg", ImageSize = "128 × 128", Type = "JPEG イメージ です。\n長い説明をここに\n表示します。", Size = "☆☆☆☆☆", CreateDate = "2011/11/11 11:1" + i.ToString() });
            }

            this.DataContext = mv;
        }

        void ListSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ListViewFile lbf = ((sender as ListBox).SelectedItem as ListViewFile);
            MessageBox.Show(lbf.CreateDate);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
