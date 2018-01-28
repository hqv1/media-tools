using Hqv.MediaTools.Types.Thumbnail;

namespace Hqv.MediaTools.Thumbnail
{
    public class Response : ThumbnailCreateResponse
    {
        public Response(ThumbnailCreationRequest request) : base(request)
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