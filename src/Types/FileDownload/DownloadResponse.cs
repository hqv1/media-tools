using Hqv.Seedwork.Components;

namespace Hqv.MediaTools.Types.FileDownload
{
    public class DownloadResponse : ResponseBase
    {
        public DownloadResponse(DownloadRequest request)
            : base((RequestBase) request)
        {
        }
    }
}