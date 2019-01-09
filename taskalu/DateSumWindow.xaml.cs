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
    /// DateSumWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DateSumWindow : Window
    {
        public DateSumWindow()
        {
            InitializeComponent();
            this.DataContext = DateSumViewModel.dsv;

            DateTime dt = DateTime.Today;
            textboxDateSum.Text = dt.ToString("d", System.Globalization.CultureInfo.CurrentCulture);

            ExecuteFirstSelectTableTaskTime(dt.Date);

        }

        private void ExecuteFirstSelectTableTaskTime(DateTime dt)
        {
            DateSumViewModel.dsv.Entries.Clear();
            SQLiteClass.DateSumMoreCount = 0;
            if (SQLiteClass.ExecuteFirstSelectTableTaskTime(dt))
            {
                DateSumMoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                DateSumMoreButton.Visibility = Visibility.Collapsed;
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

            dlg.dateString = textboxDateSum.Text;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // due date window is closed
                textboxDateSum.Text = dlg.dateString;
                ExecuteFirstSelectTableTaskTime(
                    DateTime.ParseExact(
                        textboxDateSum.Text,
                        "d", 
                        System.Globalization.CultureInfo.CurrentCulture)
                        .Date);
            }
        }

        /// <summary>
        /// more button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateSumMoreButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.ParseExact(
                textboxDateSum.Text,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture).Date;

            if (SQLiteClass.ExecuteMoreSelectTableTaskTime(dt))
            {
                DateSumMoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                DateSumMoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Name_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.DateSumNameOrderByDirection);
            SQLiteClass.DateSumSetOrderBy("name", SQLiteClass.DateSumNameOrderByDirection);
            ExecuteFirstSelectTableTaskTime(
                DateTime.ParseExact(
                    textboxDateSum.Text,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)
                    .Date);
        }
        private void NameAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "ASC";
            SQLiteClass.DateSumSetOrderBy("name", "ASC");
            ExecuteFirstSelectTableTaskTime(
                DateTime.ParseExact(
                    textboxDateSum.Text,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)
                    .Date);
        }
        private void NameDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "DESC";
            SQLiteClass.DateSumSetOrderBy("name", "DESC");
            ExecuteFirstSelectTableTaskTime(
                DateTime.ParseExact(
                    textboxDateSum.Text,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)
                    .Date);
        }

        private void Duration_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = SQLiteClass.toggleDirection(SQLiteClass.DateSumNameOrderByDirection);
            SQLiteClass.DateSumSetOrderBy("duration", SQLiteClass.DateSumNameOrderByDirection);
            ExecuteFirstSelectTableTaskTime(
                DateTime.ParseExact(
                    textboxDateSum.Text,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)
                    .Date);
        }
        private void DurationAsc_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "ASC";
            SQLiteClass.DateSumSetOrderBy("duration", "ASC");
            ExecuteFirstSelectTableTaskTime(
                DateTime.ParseExact(
                    textboxDateSum.Text,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)
                    .Date);
        }
        private void DurationDes_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.DateSumNameOrderByDirection = "DESC";
            SQLiteClass.DateSumSetOrderBy("duration", "DESC");
            ExecuteFirstSelectTableTaskTime(
                DateTime.ParseExact(
                    textboxDateSum.Text,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)
                    .Date);
        }

        private void DateSumCopyButton_Click(object sender, RoutedEventArgs e)
        {
            ClipBrd.CopyDsvToClipBoard();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
