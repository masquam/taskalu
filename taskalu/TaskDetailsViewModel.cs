using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Taskalu
{
    public class TaskDetailsViewModel
    {
        public static TaskDetailsViewModel tdv = new TaskDetailsViewModel();

        public TaskDetailsViewModel()
        {
            ObservableCollection<ListTaskDetails> entries = new ObservableCollection<ListTaskDetails>();
            Entries = entries;
        }

        public ObservableCollection<ListTaskDetails> Entries { get; set; }

        private ListTaskDetails selectedEntry;

        public ListTaskDetails SelectedEntry
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
