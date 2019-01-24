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
    /// TemplateDetailsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TemplateDetailsWindow : Window
    {
        public ListTemplate theTemplate= new ListTemplate();

        public TemplateDetailsWindow()
        {
            InitializeComponent();
        }

        private void templateDetailsLoaded(object sender, RoutedEventArgs e)
        {
            templateName.Text = theTemplate.Name;
            templateDescription.Text = theTemplate.Template;
        }

        private void ButtonTemplateDetailsOk_Click(object sender, RoutedEventArgs e)
        {
            theTemplate.Name = templateName.Text;
            theTemplate.Template = templateDescription.Text;

            this.DialogResult = true;
        }
    }
}
