using System;
using Hqv.CSharp.Common.Components;

namespace Hqv.MediaTools.Types.Thumbnail
{
    public class ThumbnailCreationRequest : RequestBase
    {
        public ThumbnailCreationRequest(string videoPath, int getThumbnailEveryNSeconds, string correlationId = null)
            : base(correlationId ?? Guid.NewGuid().ToString())
        {
            VideoPath = videoPath;
            GetThumbnailEveryNSeconds = getThumbnailEveryNSeconds;
        }

        public string VideoPath { get; }
        public int GetThumbnailEveryNSeconds { get; }
    }
}