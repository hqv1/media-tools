namespace Hqv.MediaTools.Types.ThumbnailSheet
{
    /// <summary>
    /// Service for creation of thumbnail sheet. 
    /// </summary>
    public interface IThumbnailSheetCreationService
    {
        /// <summary>
        /// Create a thumbnailnail sheet.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The response</returns>
        ThumbnailSheetCreateResponse Create(ThumbnailSheetCreateRequest request);
    }
}