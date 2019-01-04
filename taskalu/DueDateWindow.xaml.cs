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
    /// DueDateWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DueDateWindow : Window
    {
        public string dueDateString;

        public DueDateWindow()
        {
            InitializeComponent();
        }

        private void DueDateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime due = DateTime.ParseExact(dueDateString, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DueDateBox.SelectedDate = due;
            DueDateHourBox.SelectedIndex = Int32.Parse(due.ToString("HH"));
            DueDateMinuteBox.SelectedIndex = Int32.Parse(due.ToString("mm")) / 5;
        }

        private void ButtonDueDateWindowOk_Click(object sender, RoutedEventArgs e)
        {
            DateTime due = (DateTime)DueDateBox.SelectedDate;
            DateTime dueDate = new DateTime(due.Year, due.Month, due.Day, DueDateHourBox.SelectedIndex, (DueDateMinuteBox.SelectedIndex * 5), 0);
            dueDateString = dueDate.ToString("yyyy-MM-dd HH:mm:ss");

            // Dialog box accepted; ウィンドウを閉じる
            this.DialogResult = true;
        }


    }
}
