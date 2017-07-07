namespace Hqv.MediaTools.Domain
{
    public class VideoFileInfoRequest
    {
        public VideoFileInfoRequest(string videoFilePath)
        {
            VideoFilePath = videoFilePath;
        }

        public string VideoFilePath { get;  }
    }
}