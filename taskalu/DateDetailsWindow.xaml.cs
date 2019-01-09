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
    /// DateDetailsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DateDetailsWindow : Window
    {
        public DateDetailsWindow()
        {
            InitializeComponent();
            this.DataContext = DateDetailsViewModel.dsv;

            DateTime dt = DateTime.Today;
            textboxDateDetails.Text = DateCalc.DateToString(dt);

            ExecuteFirstSelectTableTaskTime(dt.Date);
        }

        private void ExecuteFirstSelectTableTaskTime(DateTime dt)
        {
            DateDetailsViewModel.dsv.Entries.Clear();
            SQLiteClass.DateDetailsMoreCount = 0;
            if (SQLiteClass.ExecuteFirstSelecttDateDetailsTableTaskTime(dt))
            {
                DateDetailsMoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                DateDetailsMoreButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// date change button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDateDetailsChange_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            DateSumDateWindow dlg = new DateSumDateWindow();

            // Configure the dialog box
            dlg.Owner = this;

            dlg.dateString = textboxDateDetails.Text;

            // Open the dialog box modally 
            if (dlg.ShowDialog() == true)
            {
                // due date window is closed
                textboxDateDetails.Text = dlg.dateString;
                ExecuteFirstSelectTableTaskTime(DateCalc.StringToDate(textboxDateDetails.Text));
            }
        }

        /// <summary>
        /// more button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateDetailsMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SQLiteClass.ExecuteMoreSelectDateDetailsTableTaskTime(DateCalc.StringToDate(textboxDateDetails.Text)))
            {
                DateDetailsMoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                DateDetailsMoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Name_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NameAsc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NameDes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartDate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartDateAsc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartDateDes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EndDate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EndDateAsc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EndDateDes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Duration_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DurationAsc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DurationDes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DateDetailsCopyButton_Click(object sender, RoutedEventArgs e)
        {
            ClipBrd.CopyDsvDateDetailsToClipBoard();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
