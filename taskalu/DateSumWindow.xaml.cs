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

            ExecuteFirstSelectTableTaskTime();

        }

        private void ExecuteFirstSelectTableTaskTime()
        {
            DateSumViewModel.dsv.Entries.Clear();
            SQLiteClass.DateSumMoreCount = 0;
            if (SQLiteClass.ExecuteFirstSelectTableTaskTime())
            {
                // TODO: more 実装
                //MoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                // TODO: more 実装
                //MoreButton.Visibility = Visibility.Collapsed;
            }
        }


        private void buttonDateSumChange_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
        }

        private void DateSumMoreButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 実装
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
