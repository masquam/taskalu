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
            textboxDateSum.Text = dt.ToString("yyyy-MM-dd");

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
                        dlg.dateString,
                        "yyyy-MM-dd", 
                        System.Globalization.CultureInfo.InvariantCulture)
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
            // TODO: 実装
        }

        private void Duration_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
        }

        private void DueDate_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
        }

        private void NameAsc_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
        }

        private void NameDes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
        }

        private void DurationAsc_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
        }

        private void DurationDes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
        }

    }
}
