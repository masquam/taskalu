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
    /// DescriptionWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DescriptionWindow : Window
    {
        public string descriptionString = "";

        public DescriptionWindow()
        {
            InitializeComponent();
        }

        private void DescriptionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DescriptionTextBox.Text = descriptionString;
        }

        private void DescriptionOk_Click(object sender, RoutedEventArgs e)
        {
            descriptionString = DescriptionTextBox.Text;
            this.DialogResult = true;
        }
    }
}
