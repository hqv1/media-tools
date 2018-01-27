using System.Threading.Tasks;

namespace Hqv.MediaTools.Types.FileDownload
{
    /// <summary>
    /// Download a file
    /// </summary>
    public interface IFileDownloaderService
    {
        Task<DownloadResponse> Download(DownloadRequest request);
    }
    
}