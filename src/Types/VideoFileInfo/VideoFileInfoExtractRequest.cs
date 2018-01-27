using System;
using Hqv.Seedwork.Components;

namespace Hqv.MediaTools.Types.VideoFileInfo
{
    /// <summary>
    /// IVideoFileInfo Extract Request
    /// </summary>
    public class VideoFileInfoExtractRequest : RequestBase
    {
        public VideoFileInfoExtractRequest(string videoFilePath, string correlationId = null)
            : base(correlationId?? Guid.NewGuid().ToString())
        {
            VideoFilePath = videoFilePath;
        }        

        /// <summary>
        /// Video file path
        /// </summary>
        public string VideoFilePath { get;  }
    }
}