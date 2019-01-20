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
    /// MemoAddWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MemoAddWindow : Window
    {
        public string memoString = "";

        public MemoAddWindow()
        {
            InitializeComponent();
        }

        private void AddMemoOk_Click(object sender, RoutedEventArgs e)
        {
            memoString = AddMemoTextBox.Text;
            this.DialogResult = true;
        }
    }
}
