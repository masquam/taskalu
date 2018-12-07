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
using System.Windows.Shapes;

namespace Taskalu
{
    /// <summary>
    /// NewWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NewWindow: Window
    {
        public NewWindow()
        {
            InitializeComponent();
        }

        private void ButtonNewWindowOk_Click(object sender, RoutedEventArgs e)
        {

            /*
            // TODO: DB書き込みに変更
            MainViewModel.mv.Files.Add(new ListViewFile()
            {
                Name = NewTitleBox.Text,
                Description = NewDesriptioncBox.Text,
                Priority = "☆☆☆☆☆",
                CreateDate = "2011/11/11 11:10",
                Id = "11"
            });
            */
            ListViewFile lvFile = new ListViewFile()
            {
                Name = NewTitleBox.Text,
                Description = NewDesriptioncBox.Text,
                Priority = "\u272e\u272e\u272e\u272e\u272e",
                //CreateDate = "2011/11/11 11:10",
                Id = "11"
            };
            SQLiteClass.ExecuteInsertTable(lvFile);

            // Dialog box accepted; ウィンドウを閉じる
            this.DialogResult = true;
        }
    }
}
