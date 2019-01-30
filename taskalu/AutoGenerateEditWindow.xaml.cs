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
    /// AutoGenerateEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AutoGenerateEditWindow : Window
    {
        public AutoGenerateEditWindow()
        {
            InitializeComponent();

            AutoGenerateList.DataContext = AutoGenerateListViewModel.aglv;

            AutoGenerateListViewModel.aglv.Entries.Clear();
            SQLiteClass.ExecuteSelectTableAutoGenerate(SQLiteClass.dbpath, AutoGenerateListViewModel.aglv);

        }

        private void AutoGenerateEditOk_Click(object sender, RoutedEventArgs e)
        {
            Int64 newOrder = 0;
            foreach (ListAutoGenerate entry in AutoGenerateListViewModel.aglv.Entries)
            {
                entry.Order = newOrder;
                SQLiteClass.ExecuteUpdateTableAutoGenerate(SQLiteClass.dbpath, entry);
                newOrder++;
            }
            this.DialogResult = true;
        }

        private void TriangleButton_AutoGenerate_Up_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = AutoGenerateList.SelectedIndex;
            if (currentIndex > 0)
            {
                AutoGenerateListViewModel.aglv.Entries.Move(currentIndex, currentIndex - 1);
            }
        }

        private void TriangleButton_AutoGenerate_Down_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = AutoGenerateList.SelectedIndex;
            if ((currentIndex >= 0) &&
                (currentIndex < AutoGenerateListViewModel.aglv.Entries.Count - 1))
            {
                AutoGenerateListViewModel.aglv.Entries.Move(currentIndex, currentIndex + 1);
            }
        }

        private void EditTheAutoGenerate_Click(object sender, RoutedEventArgs e)
        {
            int ind = AutoGenerateList.SelectedIndex;
            if (ind >= 0)
            {
                AutoGenerateDetailsWindow dlg = new AutoGenerateDetailsWindow();
                dlg.Owner = this;
                dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dlg.theAutoGenerate.Id = AutoGenerateListViewModel.aglv.Entries[ind].Id;
                dlg.theAutoGenerate.Order = AutoGenerateListViewModel.aglv.Entries[ind].Order;
                dlg.theAutoGenerate.Type = AutoGenerateListViewModel.aglv.Entries[ind].Type;
                dlg.theAutoGenerate.Name = AutoGenerateListViewModel.aglv.Entries[ind].Name;
                dlg.theAutoGenerate.Priority = AutoGenerateListViewModel.aglv.Entries[ind].Priority;
                dlg.theAutoGenerate.Template = AutoGenerateListViewModel.aglv.Entries[ind].Template;
                dlg.theAutoGenerate.Number1 = AutoGenerateListViewModel.aglv.Entries[ind].Number1;
                dlg.theAutoGenerate.Number2 = AutoGenerateListViewModel.aglv.Entries[ind].Number2;
                dlg.theAutoGenerate.Due_hour = AutoGenerateListViewModel.aglv.Entries[ind].Due_hour;
                dlg.theAutoGenerate.Due_minute = AutoGenerateListViewModel.aglv.Entries[ind].Due_minute;

                if (dlg.ShowDialog() == true)
                {
                    SQLiteClass.ExecuteUpdateTableAutoGenerate(SQLiteClass.dbpath, dlg.theAutoGenerate);

                    AutoGenerateListViewModel.aglv.Entries.Clear();
                    SQLiteClass.ExecuteSelectTableAutoGenerate(SQLiteClass.dbpath, AutoGenerateListViewModel.aglv);
                }
            }
        }

        private void DeleteTheAutoGenerate_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = AutoGenerateList.SelectedIndex;
            if (currentIndex >= 0)
            {
                var result = MessageBox.Show(Properties.Resources.TE_DeleteCaution, "taskalu",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Int64 id = AutoGenerateListViewModel.aglv.Entries[currentIndex].Id;
                    if (SQLiteClass.ExecuteDeleteTableAutoGenerate(SQLiteClass.dbpath, id))
                    {
                        AutoGenerateListViewModel.aglv.Entries.RemoveAt(currentIndex);
                    }
                }
            }
        }

        private void AddNewAutoGenerate_Click(object sender, RoutedEventArgs e)
        {
            AutoGenerateDetailsWindow dlg = new AutoGenerateDetailsWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dlg.theAutoGenerate.Id = 0;
            dlg.theAutoGenerate.Order = SQLiteClass.ExecuteSelectMaxAutoGenerate(SQLiteClass.dbpath) + 1;
            dlg.theAutoGenerate.Type = (Int64)ListAutoGenerate.TypeName.A_Day_Of_Every_Month;
            dlg.theAutoGenerate.Name = "Auto Generated Task";
            dlg.theAutoGenerate.Priority = "\u272e\u272e\u272e\u272e\u272e";
            dlg.theAutoGenerate.Template = 0;
            dlg.theAutoGenerate.Number1 = 1;
            dlg.theAutoGenerate.Number2 = 0;
            dlg.theAutoGenerate.Due_hour = 17;
            dlg.theAutoGenerate.Due_minute = 0;

            if (dlg.ShowDialog() == true)
            {
                SQLiteClass.ExecuteInsertTableAutoGenerate(SQLiteClass.dbpath, dlg.theAutoGenerate);
                //
                AutoGenerateListViewModel.aglv.Entries.Clear();
                SQLiteClass.ExecuteSelectTableAutoGenerate(SQLiteClass.dbpath, AutoGenerateListViewModel.aglv);
            }
        }
    }
}
