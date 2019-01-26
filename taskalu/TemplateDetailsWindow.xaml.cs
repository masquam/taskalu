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
using Microsoft.Win32;

namespace Taskalu
{
    /// <summary>
    /// TemplateDetailsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TemplateDetailsWindow : Window
    {
        public ListTemplate theTemplate= new ListTemplate();

        public TemplateDetailsWindow()
        {
            InitializeComponent();

            TemplatePathList.DataContext = TemplatePathListViewModel.tplv;
        }

        private void templateDetailsLoaded(object sender, RoutedEventArgs e)
        {
            templateName.Text = theTemplate.Name;
            templateDescription.Text = theTemplate.Template;

            if (theTemplate.Id == 0)
            {
                EditTemplatePathButtonsPanel.Visibility = Visibility.Collapsed;
                EditTemplatePathPanel.Visibility = Visibility.Collapsed;
                NewTemplatePathPanel.Visibility = Visibility.Visible;
            }
            else
            {
                NewTemplatePathPanel.Visibility = Visibility.Collapsed;
                EditTemplatePathButtonsPanel.Visibility = Visibility.Visible;
                EditTemplatePathPanel.Visibility = Visibility.Visible;

                TemplatePathListViewModel.tplv.Entries.Clear();
                SQLiteClass.ExecuteSelectTableTemplatePath(TemplatePathListViewModel.tplv, theTemplate.Id);
            }
        }

        private void ButtonTemplateDetailsOk_Click(object sender, RoutedEventArgs e)
        {
            theTemplate.Name = templateName.Text;
            theTemplate.Template = templateDescription.Text;

            Int64 newOrder = 0;
            foreach (ListTemplatePath entry in TemplatePathListViewModel.tplv.Entries)
            {
                newOrder++;
                entry.Order = newOrder;
                SQLiteClass.ExecuteUpdateTableTemplatePath(entry);
            }

            this.DialogResult = true;
        }

        private void TriangleButton_TemplatePath_Up_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = TemplatePathList.SelectedIndex;
            if (currentIndex > 0)
            {
                TemplatePathListViewModel.tplv.Entries.Move(currentIndex, currentIndex - 1);
            }
        }

        private void TriangleButton_TemplatePath_Down_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = TemplatePathList.SelectedIndex;
            if ((currentIndex >= 0) &&
                (currentIndex < TemplatePathListViewModel.tplv.Entries.Count - 1))
            {
                TemplatePathListViewModel.tplv.Entries.Move(currentIndex, currentIndex + 1);
            }
        }

        private void DeleteTheTemplatePath_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = TemplatePathList.SelectedIndex;
            if (currentIndex >= 0)
            {
                MessageBox.Show(TemplatePathListViewModel.tplv.Entries[currentIndex].Path);

                var result =  MessageBox.Show(Properties.Resources.TE_DeleteCaution, "taskalu",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (SQLiteClass.ExecuteDeleteTableTemplatePath(TemplatePathListViewModel.tplv.Entries[currentIndex].Id))
                    {
                        TemplatePathListViewModel.tplv.Entries.RemoveAt(currentIndex);
                    }
                }
            }
        }

        private void AddNewTemplatePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                //MessageBox.Show(dlg.FileName);
                ListTemplatePath newTP = new ListTemplatePath();
                newTP.Id = 0; //dummy
                newTP.Template_Id = theTemplate.Id;
                newTP.Order = SQLiteClass.ExecuteSelectMaxTemplatePath(theTemplate.Id) + 1;
                newTP.Path = dlg.FileName;

                SQLiteClass.ExecuteInsertTableTemplatePath(newTP);

                TemplatePathListViewModel.tplv.Entries.Clear();
                SQLiteClass.ExecuteSelectTableTemplatePath(TemplatePathListViewModel.tplv, theTemplate.Id);
            }
        }

    }
}
