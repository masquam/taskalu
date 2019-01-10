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
    /// LanguageSettingsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LanguageSettingsWindow : Window
    {
        public static string language;

        public LanguageSettingsWindow()
        {
            InitializeComponent();
        }

        private void LanguageOk_Click(object sender, RoutedEventArgs e)
        {
            language = LanguageBox.SelectedValue.ToString();
            this.DialogResult = true;
        }

        private void LanguageSettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (language == "Japanese")
            {
                LanguageBox.SelectedIndex = 1;
            }
        }
    }
}
