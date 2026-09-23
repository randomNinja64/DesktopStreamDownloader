using System;

namespace DesktopStreamDownloader
{
    public class Download
    {
        private Uri DownloadUrl;
        private string FileName;
        private string DownloadProgress;

        // Create properties
        public Uri downloadUrl
        {
            get { return DownloadUrl; }
            set { DownloadUrl = value; }
        }
        public string downloadProgress
        {
            get { return DownloadProgress; }
            set { DownloadProgress = value; }
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
    }
}
