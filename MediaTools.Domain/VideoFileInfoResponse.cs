using Hqv.CSharp.Common.Components;
using Hqv.MediaTools.Domain.Entities;

namespace Hqv.MediaTools.Domain
{
    /// <summary>
    /// IVideoFileInfo Response
    /// </summary>
    public class VideoFileInfoResponse : ResponseBase
    {
        public VideoFileInfoResponse(VideoFileInfoRequest request)
        {
            Request = request;
        }

        /// <summary>
        /// Request
        /// </summary>
        public VideoFileInfoRequest Request { get; }

        /// <summary>
        /// Video file information
        /// </summary>
        public VideoFileInformationEntity VideoFileInformation { get; set; }
    }
}