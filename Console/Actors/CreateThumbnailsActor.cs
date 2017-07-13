using System;
using Hqv.CSharp.Common.Audit;
using Hqv.MediaTools.Types.Thumbnail;

namespace Hqv.MediaTools.Console.Actors
{
    /// <summary>
    /// Create thumbnails. Not threadsafe
    /// </summary>
    internal class CreateThumbnailsActor
    {
        private readonly IAuditorResponseBase _auditor;
        private readonly IThumbnailCreationService _thumbnailCreationService;
        private string _correlationId;

        public CreateThumbnailsActor(
            IAuditorResponseBase auditor,
            IThumbnailCreationService thumbnailCreationService)
        {
            _auditor = auditor;
            _thumbnailCreationService = thumbnailCreationService;
        }

        public int Act(CreateThumbnailsOptions options)
        {
            _correlationId = Guid.NewGuid().ToString();

            var request = new ThumbnailCreationRequest(options.VideoFilePath, options.ThumbnailsEverySecond, _correlationId);
            var response = _thumbnailCreationService.Create(request);

            if (response.IsValid)
            {
                _auditor.AuditSuccess("VideoFile", options.VideoFilePath, "ThumbnailsCreated", response);
            }
            else
            {
                _auditor.AuditFailure("VideoFile", options.VideoFilePath, "ThumbnailCreationFailed", response);
            }
            return response.IsValid ? 0 : 1;
        }
    }
}