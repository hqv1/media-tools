using Hqv.CSharp.Common.Components;
using Hqv.MediaTools.Domain.Entities;

namespace Hqv.MediaTools.Domain
{
    /// <summary>
    /// IVideoFileInfo Extract Response
    /// </summary>
    public class VideoFileInfoExtractResponse : ResponseBase
    {
        public VideoFileInfoExtractResponse(VideoFileInfoExtractRequest request)
        {
            Request = request;
        }

        /// <summary>
        /// Original request
        /// </summary>
        public VideoFileInfoExtractRequest Request { get; }

        /// <summary>
        /// Video file information
        /// </summary>
        public VideoFileInformationEntity VideoFileInformation { get; set; }
    }
}