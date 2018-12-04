using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Taskalu
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public static MainViewModel mv = new MainViewModel();

        public MainViewModel()
        {
            ObservableCollection<ListViewFile> files = new ObservableCollection<ListViewFile>();
            Files = files;
/*
            try
            {
                SelectedFile = files[0];
            } catch (Exception) {
                SelectedFile = null;
            }
*/
        }

        public ObservableCollection<ListViewFile> Files { get; set; }

        private ListViewFile selectedFile;

        public ListViewFile SelectedFile
        {
            get
            {
                return selectedFile;
            }
            set
            {
                selectedFile = value;
                NotifyPropertyChanged("SelectedFile");
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
