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
            SQLiteClass.ExecuteSelectTableTemplate(SQLiteClass.dbpath, TemplateListViewModel.tlv);
        }

        private void ButtonTemplateEditOk_Click(object sender, RoutedEventArgs e)
        {
            saveOrderOfTemplate(SQLiteClass.dbpath, TemplateListViewModel.tlv);
            this.DialogResult = true;
        }

        private void saveOrderOfTemplate(string dbpath, TemplateListViewModel tlv)
        {
            Int64 newOrder = 0;
            foreach (ListTemplate entry in tlv.Entries)
            {
                entry.Order = newOrder;
                SQLiteClass.ExecuteUpdateTableTemplate(dbpath, entry);
                newOrder++;
            }
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
                    SQLiteClass.ExecuteUpdateTableTemplate(SQLiteClass.dbpath, dlg.theTemplate);

                    saveOrderOfTemplate(SQLiteClass.dbpath, TemplateListViewModel.tlv);

                    TemplateListViewModel.tlv.Entries.Clear();
                    SQLiteClass.ExecuteSelectTableTemplate(SQLiteClass.dbpath, TemplateListViewModel.tlv);
                }
            }
        }

        private void DeleteTheTemplate_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = TemplateList.SelectedIndex;
            if (currentIndex >= 0)
            {
                var result = MessageBox.Show(Properties.Resources.TE_DeleteCaution, "taskalu",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Int64 id = TemplateListViewModel.tlv.Entries[currentIndex].Id;
                    if (SQLiteClass.ExecuteDeleteTableTemplate(SQLiteClass.dbpath, id))
                    {
                        SQLiteClass.ExecuteDeleteTableTemplatePathFromTemplateId(SQLiteClass.dbpath, id);

                        TemplateListViewModel.tlv.Entries.RemoveAt(currentIndex);
                    }
                }
            }
        }

        private void AddNewTemplate_Click(object sender, RoutedEventArgs e)
        {
            TemplateDetailsWindow dlg = new TemplateDetailsWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dlg.theTemplate.Id = 0;
            dlg.theTemplate.Order = SQLiteClass.ExecuteSelectMaxTemplate(SQLiteClass.dbpath) + 1;
            dlg.theTemplate.Name = "";
            dlg.theTemplate.Template = "";

            if (dlg.ShowDialog() == true)
            {
                saveOrderOfTemplate(SQLiteClass.dbpath, TemplateListViewModel.tlv);

                SQLiteClass.ExecuteInsertTableTemplate(SQLiteClass.dbpath, dlg.theTemplate);
                
                TemplateListViewModel.tlv.Entries.Clear();
                SQLiteClass.ExecuteSelectTableTemplate(SQLiteClass.dbpath, TemplateListViewModel.tlv);
            }
        }
    }
}
