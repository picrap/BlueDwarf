
namespace BlueDwarf.ViewModel
{
    using System.IO;
    using Aspects;
    using Microsoft.Practices.Unity;
    using Navigation;

    /// <summary>
    /// Simple file download using IE ActiveX control
    /// We're not supposed to know this here, so that's going to be our little secret...
    /// </summary>
    public class WebDownloaderViewModel : ViewModel
    {
        [Dependency]
        public INavigator Navigator { get; set; }

        [NotifyPropertyChanged]
        public string DownloadUri { get; set; }

        [NotifyPropertyChanged]
        public string SaveTextPath { get; set; }

        private string _documentText;
        [NotifyPropertyChanged]
        public string DocumentText
        {
            get { return _documentText; }
            set
            {
                _documentText = value;
                OnDocumentTextChanged();
            }
        }

        private void OnDocumentTextChanged()
        {
            if (SaveTextPath != null)
            {
                var parentDirectory = Path.GetDirectoryName(SaveTextPath);
                if (!Directory.Exists(parentDirectory))
                    Directory.CreateDirectory(parentDirectory);
                using (var streamWriter = File.CreateText(SaveTextPath))
                    streamWriter.Write(DocumentText);
            }
            Navigator.Exit(false);
        }
    }
}
