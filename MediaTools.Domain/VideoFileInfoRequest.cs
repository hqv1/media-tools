namespace Hqv.MediaTools.Domain
{
    /// <summary>
    /// IVideoFileInfo Request
    /// </summary>
    public class VideoFileInfoRequest
    {
        public VideoFileInfoRequest(string videoFilePath)
        {
            VideoFilePath = videoFilePath;
        }

        /// <summary>
        /// Video file path
        /// </summary>
        public string VideoFilePath { get;  }
    }
}