using Hqv.CSharp.Common.Components;
using Hqv.MediaTools.Types.Entities;

namespace Hqv.MediaTools.Types.VideoFileInfo
{
    /// <summary>
    /// IVideoFileInfo Extract Response
    /// </summary>
    public class VideoFileInfoExtractResponse : ResponseBase
    {
        public VideoFileInfoExtractResponse(VideoFileInfoExtractRequest request)
            :base(request)
        {           
        }
        
        /// <summary>
        /// Video file information
        /// </summary>
        public VideoFileInformationEntity VideoFileInformation { get; set; }

        
    }
}