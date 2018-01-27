namespace Hqv.MediaTools.Types.Thumbnail
{
    public interface IThumbnailCreationService
    {
        ThumbnailCreateResponse Create(ThumbnailCreationRequest request);
    }
}