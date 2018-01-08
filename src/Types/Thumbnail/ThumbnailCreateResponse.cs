using Hqv.CSharp.Common.Components;

namespace Hqv.MediaTools.Types.Thumbnail
{
    public class ThumbnailCreateResponse : ResponseBase
    {
        public ThumbnailCreateResponse(ThumbnailCreationRequest request)
            : base(request)
        {
        }
    }
}