using Hqv.CSharp.Common.Components;
using Hqv.MediaTools.Domain.Entities;

namespace Hqv.MediaTools.Domain
{
    public class VideoFileInfoResponse : ResponseBase
    {
        public VideoFileInfoResponse(VideoFileInfoRequest request)
        {
            Request = request;
        }

        public VideoFileInfoRequest Request { get; }

        public VideoFileInformationEntity VideoFileInformation { get; set; }
    }
}