using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Taskalu
{
    public class TemplatePathListViewModel
    {
        public static TemplatePathListViewModel tplv = new TemplatePathListViewModel();

        public TemplatePathListViewModel()
        {
            ObservableCollection<ListTemplatePath> entries = new ObservableCollection<ListTemplatePath>();
            Entries = entries;
        }

        public ObservableCollection<ListTemplatePath> Entries { get; set; }

        private ListTemplate selectedEntry;

        public ListTemplate SelectedEntry
        {
            get
            {
                return selectedEntry;
            }
            set
            {
                selectedEntry = value;
                NotifyPropertyChanged("SelectedEntry");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

    }
}
