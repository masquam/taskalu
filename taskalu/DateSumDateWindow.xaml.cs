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
            DateTime due = DateCalc.StringToDate(dateString, System.Globalization.CultureInfo.CurrentCulture);
            DateSumDateBox.SelectedDate = due;
        }

        private void ButtonDateSumDateWindowOk_Click(object sender, RoutedEventArgs e)
        {
            DateTime due = (DateTime)DateSumDateBox.SelectedDate;
            dateString = DateCalc.DateToString(due);

            // Dialog box accepted; ウィンドウを閉じる
            this.DialogResult = true;
        }

        private void TriangleButton_Up_Click(object sender, RoutedEventArgs e)
        {
            DateSumDateBox.SelectedDate -= new TimeSpan(1, 0, 0, 0);
        }

        private void TriangleButton_Down_Click(object sender, RoutedEventArgs e)
        {
            DateSumDateBox.SelectedDate += new TimeSpan(1, 0, 0, 0);
        }
    }
}
