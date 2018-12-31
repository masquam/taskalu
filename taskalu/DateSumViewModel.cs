using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Taskalu
{
    public class DateSumViewModel : INotifyPropertyChanged
    {
        public static DateSumViewModel dsv = new DateSumViewModel();

        public DateSumViewModel()
        {
            ObservableCollection<ListDateSum> entries = new ObservableCollection<ListDateSum>();
            Entries = entries;
        }

        public ObservableCollection<ListDateSum> Entries { get; set; }

        private ListDateSum selectedEntry;

        public ListDateSum SelectedEntry
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
