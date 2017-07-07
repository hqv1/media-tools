namespace Hqv.MediaTools.Domain
{
    public interface IVideoFileInfoService
    {
        VideoFileInfoResponse Extract(VideoFileInfoRequest request);
    }
}
