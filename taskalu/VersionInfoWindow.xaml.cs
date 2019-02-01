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
using System.Reflection;

namespace Taskalu
{
    /// <summary>
    /// VersionInfoWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class VersionInfoWindow : Window
    {
        public VersionInfoWindow()
        {
            InitializeComponent();

            Assembly thisAssem = Assembly.GetExecutingAssembly();
            AssemblyName thisAssemName = thisAssem.GetName();
            Version ver = thisAssemName.Version;
            versionLabel.Content = ver.ToString();
        }

        private void LicenseClicked(object sender, RoutedEventArgs e)
        {
            LicenseWindow dlg = new LicenseWindow();
            dlg.Owner = this;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dlg.ShowDialog() == true)
            {
                //
            }
        }
    }
}
