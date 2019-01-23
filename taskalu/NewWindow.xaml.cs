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

            TimeSpan ts1hour = TimeSpan.FromHours(1);
            DateTime due = System.DateTime.Now + ts1hour;
            dateBox.SelectedDate = due;

            hourBox.SelectedIndex = Int32.Parse(due.ToString("HH"));
            minuteBox.SelectedIndex = Int32.Parse(due.ToString("mm")) / 5;
        }

        private void ButtonNewWindowOk_Click(object sender, RoutedEventArgs e)
        {
            Int64 retId;

            NewWindowOk.IsEnabled = false;

            DateTime due = (DateTime)dateBox.SelectedDate;
            DateTime dueDate = new DateTime(due.Year, due.Month, due.Day, hourBox.SelectedIndex, (minuteBox.SelectedIndex * 5), 0);

            ListViewFile lvFile = new ListViewFile()
            {
                Name = NewTitleBox.Text,
                Description = NewDescriptionBox.Text,
                Memo = "",
                Priority = priorityBox.Text,
                DueDate = TimeZoneInfo.ConvertTimeToUtc(dueDate).ToString("yyyy-MM-dd HH:mm:ss"),
                Status = "Active",
                WorkHolder = WorkHolder.CreateWorkHolder(NewTitleBox.Text)
            };
            retId = SQLiteClass.ExecuteInsertTable(lvFile);
            SQLiteClass.ExecuteInsertTableFTSString(retId, "tasklist_name", Ngram.getNgramText(NewTitleBox.Text, 2));
            SQLiteClass.ExecuteInsertTableFTSString(retId, "tasklist_description", Ngram.getNgramText(NewDescriptionBox.Text, 2));

            // Dialog box accepted; ウィンドウを閉じる
            this.DialogResult = true;

            NewWindowOk.IsEnabled = true;
        }
    }
}
