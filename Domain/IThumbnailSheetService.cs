namespace Hqv.MediaTools.Domain
{
    /// <summary>
    /// Service for thumbnail sheet. 
    /// </summary>
    public interface IThumbnailSheetService
    {
        /// <summary>
        /// Create a thumbnailnail sheet.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The response</returns>
        ThumbnailSheetCreateResponse Create(ThumbnailSheetCreateRequest request);
    }
}