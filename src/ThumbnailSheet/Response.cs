using Hqv.MediaTools.Types.ThumbnailSheet;

namespace Hqv.MediaTools.ThumbnailSheet
{
    /// <inheritdoc />
    /// <summary>
    /// Add some additional information to the response
    /// </summary>
    internal class Response : ThumbnailSheetCreateResponse
    {
        public Response(ThumbnailSheetCreateRequest request) : base(request)
        {
        }

        /// <summary>
        /// FfmpegArguments. Only populated on error
        /// </summary>
        public string FfmpegArguments { get; set; }
        /// <summary>
        /// FfmpegErrorOutput. Only populated on error
        /// </summary>
        public string FfmpegErrorOutput { get; set; }
    }
}