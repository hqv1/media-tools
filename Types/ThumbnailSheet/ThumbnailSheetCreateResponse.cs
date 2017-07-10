using Hqv.CSharp.Common.Components;

namespace Hqv.MediaTools.Types.ThumbnailSheet
{
    /// <summary>
    /// Thumbnail sheet create response
    /// </summary>
    public class ThumbnailSheetCreateResponse : ResponseBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Original request</param>
        public ThumbnailSheetCreateResponse(ThumbnailSheetCreateRequest request)
        {
            Request = request;
        }

        /// <summary>
        /// Original request
        /// </summary>
        public ThumbnailSheetCreateRequest Request { get; }        

        /// <summary>
        /// Path to the created thumbnail sheet
        /// </summary>
        public string SheetFilePath { get; set; }
    }
}