using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Taskalu
{
    public class AutoGenerateListViewModel
    {
        public static AutoGenerateListViewModel aglv = new AutoGenerateListViewModel();

        public AutoGenerateListViewModel()
        {
            ObservableCollection<ListAutoGenerate> entries = new ObservableCollection<ListAutoGenerate>();
            Entries = entries;
        }

        public ObservableCollection<ListAutoGenerate> Entries { get; set; }

        private ListAutoGenerate selectedEntry;

        public ListAutoGenerate SelectedEntry
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
