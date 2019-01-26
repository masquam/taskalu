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
    /// TemplateEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TemplateEditWindow : Window
    {
        public TemplateEditWindow()
        {
            InitializeComponent();

            TemplateList.DataContext = TemplateListViewModel.tlv;

            TemplateListViewModel.tlv.Entries.Clear();
            SQLiteClass.ExecuteSelectTableTemplate(TemplateListViewModel.tlv);
        }

        private void ButtonTemplateEditOk_Click(object sender, RoutedEventArgs e)
        {
            Int64 newOrder = 0;
            foreach(ListTemplate entry in TemplateListViewModel.tlv.Entries)
            {
                newOrder++;
                entry.Order = newOrder;
                SQLiteClass.ExecuteUpdateTableTemplate(entry);
            }

            this.DialogResult = true;
        }

        private void TriangleButton_Template_Up_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = TemplateList.SelectedIndex;
            if (currentIndex > 0) {
                TemplateListViewModel.tlv.Entries.Move(currentIndex, currentIndex - 1);
            }
        }

        private void TriangleButton_Template_Down_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = TemplateList.SelectedIndex;
            if ((currentIndex >= 0) &&
                (currentIndex < TemplateListViewModel.tlv.Entries.Count - 1))
            {
                TemplateListViewModel.tlv.Entries.Move(currentIndex, currentIndex + 1);
            }
        }

        private void EditTheTemplate_Click(object sender, RoutedEventArgs e)
        {
            int ind = TemplateList.SelectedIndex;
            if (ind >= 0)
            {
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
            }
        }

        /*
        private void TemplateSelected(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(TemplateList.SelectedIndex.ToString());
        }
        */

        private void DeleteTheTemplate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewTemplate_Click(object sender, RoutedEventArgs e)
        {
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
        }
    }
}
