using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Taskalu
{
    public class TaskMemoViewModel
    {
        public static TaskMemoViewModel tmv = new TaskMemoViewModel();

        public TaskMemoViewModel()
        {
            ObservableCollection<ListTaskMemo> memos = new ObservableCollection<ListTaskMemo>();
            Memos = memos;
        }

        public ObservableCollection<ListTaskMemo> Memos { get; set; }

        private ListTaskMemo selectedMemo;

        public ListTaskMemo SelectedMemo
        {
            get
            {
                return selectedMemo;
            }
            set
            {
                selectedMemo = value;
                NotifyPropertyChanged("SelectedMemo");
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
