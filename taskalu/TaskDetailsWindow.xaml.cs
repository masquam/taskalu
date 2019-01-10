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
    /// TaskDetailsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskDetailsWindow : Window
    {
        public static Int64 id;

        public TaskDetailsWindow()
        {
            InitializeComponent();
            this.DataContext = TaskDetailsViewModel.tdv;
        }

        private void TaskDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ExecuteFirstSelectTableTaskTimeDetails(id);
        }

        private void ExecuteFirstSelectTableTaskTimeDetails(Int64 id)
        {
            TaskDetailsViewModel.tdv.Entries.Clear();
            SQLiteClass.DateDetailsMoreCount = 0;
            if (SQLiteClass.ExecuteFirstSelecttTaskDetailsTableTaskTime(id))
            {
                TaskDetailsMoreButton.Visibility = Visibility.Visible;
                textTaskDetailsStatusBar.Content = "";
            }
            else
            {
                TaskDetailsMoreButton.Visibility = Visibility.Collapsed;
                textTaskDetailsStatusBar.Content = Properties.Resources.DD_Sum + SQLiteClass.SumTimeSpanTaskDetails.ToString(@"hh\:mm\:ss");
            }
        }

        private void TaskDetailsMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SQLiteClass.ExecuteMoreSelectTaskDetailsTableTaskTime(id))
            {
                TaskDetailsMoreButton.Visibility = Visibility.Visible;
                textTaskDetailsStatusBar.Content = "";
            }
            else
            {
                TaskDetailsMoreButton.Visibility = Visibility.Collapsed;
                textTaskDetailsStatusBar.Content = Properties.Resources.DD_Sum + SQLiteClass.SumTimeSpanTaskDetails.ToString(@"hh\:mm\:ss");
            }
        }

        private void TaskDetailsCopyButton_Click(object sender, RoutedEventArgs e)
        {
            //ClipBrd.CopyDsvTaskDetailsToClipBoard(SQLiteClass.SumTimeSpanTaskDetails);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
