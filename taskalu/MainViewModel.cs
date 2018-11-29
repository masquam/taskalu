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
        public MainViewModel()
        {
            ObservableCollection<ListViewFile> files = new ObservableCollection<ListViewFile>();

            // TODO: ここは仮データ
/*            for (int i = 0; i < 10; i++)
            {
                files.Add(new ListViewFile() { Name = "Image.jpg", ImageSize = "256 × 256", Type = "JPEG イメージ", Size = "256 KB", CreateDate = "2011/11/11 11:11" });
            }
*/
            Files = files;
            try
            {
                SelectedFile = files[0];
            } catch (Exception e) {
                SelectedFile = null;
            }
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
