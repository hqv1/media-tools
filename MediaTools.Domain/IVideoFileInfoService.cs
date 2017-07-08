namespace Hqv.MediaTools.Domain
{
    /// <summary>
    /// Extracts information from a video file
    /// </summary>
    public interface IVideoFileInfoService
    {
        /// <summary>
        /// Extracts information from a video file.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>If successful, the video file information. If not, the exception messages </returns>
        VideoFileInfoResponse Extract(VideoFileInfoRequest request);
    }
}
