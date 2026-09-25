using System;
using System.ComponentModel;

namespace DesktopStreamDownloader
{
    public class Download : INotifyPropertyChanged
    {
        private Uri DownloadUrl;
        private string FileName;
        private string DownloadStatus;
        private string DownloadProgress;
        private bool Overwrite;

        public event PropertyChangedEventHandler PropertyChanged;

        // Create properties
        public Uri downloadUrl
        {
            get { return DownloadUrl; }
            set { DownloadUrl = value; }
        }
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
        public string fileName
        {
            get { return FileName; }
            set { FileName = value; }
        }
        public bool overwrite
        {
            get { return Overwrite; }
            set { Overwrite = value; }
        }

        public Download(Uri downloadUrl, string fileName, bool overwrite)
        {
            DownloadUrl = downloadUrl;
            FileName = fileName;
            Overwrite = overwrite;
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
