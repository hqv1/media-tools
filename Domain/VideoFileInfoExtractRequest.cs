namespace Hqv.MediaTools.Domain
{
    /// <summary>
    /// IVideoFileInfo Extract Request
    /// </summary>
    public class VideoFileInfoExtractRequest
    {
        public VideoFileInfoExtractRequest(string videoFilePath)
        {
            VideoFilePath = videoFilePath;
        }

        /// <summary>
        /// Video file path
        /// </summary>
        public string VideoFilePath { get;  }
    }
}