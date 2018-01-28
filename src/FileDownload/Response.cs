using Hqv.MediaTools.Types.FileDownload;

namespace Hqv.MediaTools.FileDownload
{
    public class Response : DownloadResponse
    {
        public Response(DownloadRequest request) : base(request)
        {
        }

        /// <summary>
        /// FfmpegArguments. Only populated on error
        /// </summary>
        public string FfmpegArguments { get; set; }

        public string StandardOutput { get; set; }
        public string StandardError { get; set; }
        public int ResultCode { get; set; }
    }
}