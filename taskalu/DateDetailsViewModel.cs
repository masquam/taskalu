using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Taskalu
{
    class DateDetailsViewModel
    {
        public static DateDetailsViewModel dsv = new DateDetailsViewModel();

        public DateDetailsViewModel()
        {
            ObservableCollection<ListDateDetails> entries = new ObservableCollection<ListDateDetails>();
            Entries = entries;
        }

        public ObservableCollection<ListDateDetails> Entries { get; set; }

        private ListDateDetails selectedEntry;

        public ListDateDetails SelectedEntry
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
