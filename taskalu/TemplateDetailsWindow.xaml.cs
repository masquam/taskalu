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
                (currentIndex < TemplateListViewModel.tlv.Entries.Count - 1))
            {
                TemplatePathListViewModel.tplv.Entries.Move(currentIndex, currentIndex + 1);
            }
        }

        private void EditTheTemplatePath_Click(object sender, RoutedEventArgs e)
        {
            int ind = TemplatePathList.SelectedIndex;
            if (ind >= 0)
            {
                /*
                TemplateDetailsWindow dlg = new TemplateDetailsWindow();
                dlg.Owner = this;
                dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dlg.theTemplate.Id = TemplateListViewModel.tlv.Entries[ind].Id;
                dlg.theTemplate.Order = TemplateListViewModel.tlv.Entries[ind].Order;
                dlg.theTemplate.Name = TemplateListViewModel.tlv.Entries[ind].Name;
                dlg.theTemplate.Template = TemplateListViewModel.tlv.Entries[ind].Template;

                if (dlg.ShowDialog() == true)
                {
                    SQLiteClass.ExecuteUpdateTableTemplate(dlg.theTemplate);

                    TemplateListViewModel.tlv.Entries.Clear();
                    SQLiteClass.ExecuteSelectTableTemplate(TemplateListViewModel.tlv);
                }
                */
            }
        }

        private void DeleteTheTemplatePath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewTemplatePath_Click(object sender, RoutedEventArgs e)
        {
            /*
            TemplateDetailsWindow dlg = new TemplateDetailsWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dlg.theTemplate.Id = 0;
            dlg.theTemplate.Order = SQLiteClass.ExecuteSelectMaxTemplate() + 1;
            dlg.theTemplate.Name = "";
            dlg.theTemplate.Template = "";

            if (dlg.ShowDialog() == true)
            {
                SQLiteClass.ExecuteInsertTableTemplate(dlg.theTemplate);
                //
                TemplateListViewModel.tlv.Entries.Clear();
                SQLiteClass.ExecuteSelectTableTemplate(TemplateListViewModel.tlv);
            }
            */

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
