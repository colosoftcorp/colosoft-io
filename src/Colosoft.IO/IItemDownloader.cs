namespace Colosoft.IO
{
    public interface IItemDownloader
    {
        System.IO.Stream DownloadFile();

        void DownloadFile(string localFileName);
    }
}
