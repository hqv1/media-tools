using System;
using System.IO;
using Hqv.MediaTools.Console.Options;
using Hqv.MediaTools.Types.Models;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.MediaTools.Types.VideoFileInfo;
using Hqv.Seedwork.Audit;

namespace Hqv.MediaTools.Console.Actors
{
    /// <summary>
    /// Create thumbnail sheet. Not thread safe.
    /// </summary>
    internal class CreateThumbnailSheetActor
    {
        private readonly IAuditorResponseBase _auditor;
        private readonly IThumbnailSheetCreationService _thumbnailSheetCreationService;
        private readonly IVideoFileInfoExtractionService _videoFileInfoExtractionService;
        private string _correlationId;

        public CreateThumbnailSheetActor(
            IAuditorResponseBase auditor,
            IThumbnailSheetCreationService thumbnailSheetCreationService,
            IVideoFileInfoExtractionService videoFileInfoExtractionService)
        {
            _auditor = auditor;
            _thumbnailSheetCreationService = thumbnailSheetCreationService;
            _videoFileInfoExtractionService = videoFileInfoExtractionService;
        }

        public int Act(CreateThumbnailSheetOptions options)
        {
            _correlationId = Guid.NewGuid().ToString();
            var videoFileInfoExtractResponse = ExtractVideoFileInformation(options);
            if (!videoFileInfoExtractResponse.IsValid) return 1;
            var thumbnailSheetCreateResponse = CreateThumbnailSheet(options, videoFileInfoExtractResponse.VideoFileInformation);
            return thumbnailSheetCreateResponse.IsValid ? 0 : 1;
        }


        private VideoFileInfoExtractResponse ExtractVideoFileInformation(CreateThumbnailSheetOptions options)
        {            
            var request = new VideoFileInfoExtractRequest(options.VideoFilePath, _correlationId);            
            var response =  _videoFileInfoExtractionService.Extract(request);
            if (response.IsValid)
            {
                _auditor.AuditSuccess("VideoFile", options.VideoFilePath, "InfoExtracted", response);
            }
            else
            {
                _auditor.AuditFailure("VideoFile", options.VideoFilePath, "InfoExtractionFailed", response);
            }
            return response;
        }

        private ThumbnailSheetCreateResponse CreateThumbnailSheet(CreateThumbnailSheetOptions options, VideoFileInformationModel videoFileInformation)
        {
            var request = new ThumbnailSheetCreateRequest(
                videoPath:options.VideoFilePath,
                sheetName:Path.GetFileNameWithoutExtension(options.VideoFilePath),
                numberOfThumbnails:options.NumberOfThumbnails,
                videoDurationInSeconds:videoFileInformation.DurationInSecs,
                correlationId: _correlationId);
            var response = _thumbnailSheetCreationService.Create(request);
            if (response.IsValid)
            {
                _auditor.AuditSuccess("VideoFile", options.VideoFilePath, "ThumbnailSheetCreated", response);
            }
            else
            {
                _auditor.AuditFailure("VideoFile", options.VideoFilePath, "ThumbnailSheetCreationFailed", response);
            }
            return response;
        }
    }
}
