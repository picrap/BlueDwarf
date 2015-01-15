
namespace BlueDwarf.ViewModel
{
    using Aspects;

    public class WebDownloaderViewModel : ViewModel
    {
        [AutoNotifyPropertyChanged]
        public string DownloadUri { get; set; }

        [AutoNotifyPropertyChanged]
        public string SaveTextPath { get; set; }

        public void LoadCompleted(dynamic document)
        {
            var text = document.body.innerText;
            //dynamic doc = document;
            //var body = doc.BODY;
            //var r2 = body.createRange();
            //var t2 = r2.text;
            //dynamic selection = doc.selection;
            //dynamic range = selection.createRange();
            //var text = range.text;
        }
    }
}
