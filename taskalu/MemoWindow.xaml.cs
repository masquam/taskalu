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
    /// MemoWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MemoWindow : Window
    {
        public string date;
        public string memo;

        public MemoWindow()
        {
            InitializeComponent();
        }

        private void MemoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DateTextBox.Text = date;
            MemoTextBox.Text = memo;
        }
    }
}
