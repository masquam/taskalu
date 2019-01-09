﻿using System;
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
            DateTime due;
            DateTime.TryParseExact(
                dueDateString, "G", 
                System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.None,
                out due);
            DueDateBox.SelectedDate = due;
            DueDateHourBox.SelectedIndex = Int32.Parse(due.ToString("HH"));
            DueDateMinuteBox.SelectedIndex = Int32.Parse(due.ToString("mm")) / 5;
        }

        private void ButtonDueDateWindowOk_Click(object sender, RoutedEventArgs e)
        {
            DueDateOk.IsEnabled = false;
            DueDateCancel.IsEnabled = false;

            DateTime due = (DateTime)DueDateBox.SelectedDate;
            DateTime dueDate = new DateTime(due.Year, due.Month, due.Day, DueDateHourBox.SelectedIndex, (DueDateMinuteBox.SelectedIndex * 5), 0);
            dueDateString = dueDate.ToString("G", System.Globalization.CultureInfo.CurrentCulture);

            // Dialog box accepted; ウィンドウを閉じる
            this.DialogResult = true;

            DueDateOk.IsEnabled = true;
            DueDateCancel.IsEnabled = true;
        }


    }
}
