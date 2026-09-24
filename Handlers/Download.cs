using System;
using System.ComponentModel;

namespace DesktopStreamDownloader
{
    public class Download : INotifyPropertyChanged
    {
        private Uri DownloadUrl;
        private string FileName;
        private string DownloadProgress;

        public event PropertyChangedEventHandler PropertyChanged;

        // Create properties
        public Uri downloadUrl
        {
            get { return DownloadUrl; }
            set { DownloadUrl = value; }
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
        public string fileName
        {
            get { return FileName; }
            set { FileName = value; }
        }

        public Download() {
            this.DownloadUrl = new Uri("NULL");
            this.DownloadProgress = null;
            this.FileName = "NULL";
        }

        public Download(Uri downloadUrl, string fileName)
        {
            this.DownloadUrl = downloadUrl;
            this.DownloadProgress = null;
            this.FileName = fileName;
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
