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
    /// DateSumWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DateSumWindow : Window
    {
        public DateSumWindow()
        {
            InitializeComponent();
            this.DataContext = DateSumViewModel.dsv;

            DateTime dt = DateTime.Today;
            textboxDateSum.Text = DateCalc.DateToString(dt, System.Globalization.CultureInfo.CurrentCulture);

            ExecuteFirstSelectTableTaskTime(dt.Date);

        }

        private void ExecuteFirstSelectTableTaskTime(DateTime dt)
        {
            DateSumViewModel.dsv.Entries.Clear();
            SQLiteClass.DateSumMoreCount = 0;
            if (SQLiteClass.ExecuteFirstSelectTableTaskTime(SQLiteClass.dbpath, dt))
            {
                DateSumMoreButton.Visibility = Visibility.Visible;
                textDateSumStatusBar.Content = "";            }
            else
            {
                DateSumMoreButton.Visibility = Visibility.Collapsed;
                textDateSumStatusBar.Content = Properties.Resources.DD_Sum + SQLiteClass.SumTimeSpanDateSum.ToString(@"hh\:mm\:ss");
            }
        }

        /// <summary>
        /// date change button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDateSumChange_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            DateSumDateWindow dlg = new DateSumDateWindow();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dlg.dateString = textboxDateSum.Text;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // due date window is closed
                textboxDateSum.Text = dlg.dateString;
                ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        /// <summary>
        /// more button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateSumMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SQLiteClass.ExecuteMoreSelectTableTaskTime(SQLiteClass.dbpath, DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture)))
            {
                DateSumMoreButton.Visibility = Visibility.Visible;
                textDateSumStatusBar.Content = "";
            }
            else
            {
                DateSumMoreButton.Visibility = Visibility.Collapsed;
                textDateSumStatusBar.Content = Properties.Resources.DD_Sum + SQLiteClass.SumTimeSpanDateSum.ToString(@"hh\:mm\:ss");
            }
        }

        private void Name_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.DateSumNameOrderByDirection);
            SQLiteClass.DateSumSetOrderBy("name", SQLiteClass.DateSumNameOrderByDirection);
            ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture));
        }
        private void NameAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "ASC";
            SQLiteClass.DateSumSetOrderBy("name", "ASC");
            ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture));
        }
        private void NameDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "DESC";
            SQLiteClass.DateSumSetOrderBy("name", "DESC");
            ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture));
        }

        private void Duration_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.DateSumNameOrderByDirection);
            SQLiteClass.DateSumSetOrderBy("duration", SQLiteClass.DateSumNameOrderByDirection);
            ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture));
        }
        private void DurationAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "ASC";
            SQLiteClass.DateSumSetOrderBy("duration", "ASC");
            ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture));
        }
        private void DurationDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "DESC";
            SQLiteClass.DateSumSetOrderBy("duration", "DESC");
            ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateSum.Text, System.Globalization.CultureInfo.CurrentCulture));
        }

        private void DateSumCopyButton_Click(object sender, RoutedEventArgs e)
        {
            ClipBrd.CopyDsvToClipBoard(
                SQLiteClass.SumTimeSpanDateSum,
                DateSumMoreButton.Visibility != Visibility.Visible);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
