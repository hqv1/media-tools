using Hqv.MediaTools.Types.VideoFileInfo;

namespace Hqv.MediaTools.VideoFileInfo
{
    /// <inheritdoc />
    /// <summary>
    /// Return additional information than just the generic response
    /// </summary>
    public class Response : VideoFileInfoExtractResponse
    {
        public Response(VideoFileInfoExtractRequest request) : base(request)
        {
        }

        public string FfprobeArguments { get; set; }

        public string FfprobeOutput { get; set; }
    }
}