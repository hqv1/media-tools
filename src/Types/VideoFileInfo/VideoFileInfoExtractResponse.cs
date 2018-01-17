using Hqv.Seedwork.Components;
using Hqv.MediaTools.Types.Models;

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
        public VideoFileInformationModel VideoFileInformation { get; set; }

        
    }
}