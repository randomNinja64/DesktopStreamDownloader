using System;
using System.ComponentModel;

namespace DesktopStreamDownloader
{
    public class Download : INotifyPropertyChanged
    {
        private string DownloadStatus;
        private string DownloadProgress;

        public event PropertyChangedEventHandler PropertyChanged;

        // Create properties
        public Uri downloadUrl { get; set; }
        public string downloadStatus
        {
            get { return DownloadStatus; }
            set
            {
                if (DownloadStatus == value)
                {
                    return;
                }
                DownloadStatus = value;
                OnPropertyChanged("downloadStatus");
            }
        }
        public string downloadProgress
        {
            get { return DownloadProgress; }
            set
            {
                if (DownloadProgress == value)
                {
                    return;
                }
                DownloadProgress = value;
                OnPropertyChanged("downloadProgress");
            }
        }
        public string fileName { get; set; }
        public bool overwrite { get; set; }

        public Download(Uri downloadUrl, string fileName, bool overwrite)
        {
            this.downloadUrl = downloadUrl;
            this.fileName = fileName;
            this.overwrite = overwrite;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
