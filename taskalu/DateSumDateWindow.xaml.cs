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
    /// DateSumDateWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DateSumDateWindow : Window
    {
        public string dateString;

        public DateSumDateWindow()
        {
            InitializeComponent();
        }

        private void DateSumDateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime due = DateTime.ParseExact(dateString, "d", System.Globalization.CultureInfo.CurrentCulture);
            DateSumDateBox.SelectedDate = due;
        }

        private void ButtonDateSumDateWindowOk_Click(object sender, RoutedEventArgs e)
        {
            DateTime due = (DateTime)DateSumDateBox.SelectedDate;
            dateString = due.Date.ToString("d", System.Globalization.CultureInfo.CurrentCulture);

            // Dialog box accepted; ウィンドウを閉じる
            this.DialogResult = true;
        }
    }
}
