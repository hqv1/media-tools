using Hqv.MediaTools.Types.Models;

namespace Hqv.MediaTools.Types.VideoFileInfo
{
    /// <summary>
    /// Service for video file information
    /// </summary>
    public interface IVideoFileInfoExtractionService
    {
        /// <summary>
        /// Extracts information from a video file.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>If successful, the video file information. If not, the exception messages </returns>
        VideoFileInfoExtractResponse Extract(VideoFileInfoExtractRequest request);        
    }
}
