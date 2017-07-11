using System.IO;
using Hqv.CSharp.Common.Audit;
using Hqv.MediaTools.Types.Entities;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.MediaTools.Types.VideoFileInfo;

namespace Hqv.MediaTools.Console.Actors
{
    internal class CreateThumbnailSheetActor
    {
        private readonly IAuditorResponseBase _auditor;
        private readonly IThumbnailSheetCreationService _thumbnailSheetCreationService;
        private readonly IVideoFileInfoExtractionService _videoFileInfoExtractionService;

        public CreateThumbnailSheetActor(
            IAuditorResponseBase auditor,
            IThumbnailSheetCreationService thumbnailSheetCreationService,
            IVideoFileInfoExtractionService videoFileInfoExtractionService)
        {
            _auditor = auditor;
            _thumbnailSheetCreationService = thumbnailSheetCreationService;
            _videoFileInfoExtractionService = videoFileInfoExtractionService;
        }

        public void Act(CreateThumbnailSheetOptions options)
        {
            var videoFileInfoExtractResponse = ExtractVideoFileInformation(options);
            if (!videoFileInfoExtractResponse.IsValid) return;
            CreateThumbnailSheet(options, videoFileInfoExtractResponse.VideoFileInformation);
        }


        private VideoFileInfoExtractResponse ExtractVideoFileInformation(CreateThumbnailSheetOptions options)
        {            
            var request = new VideoFileInfoExtractRequest(options.VideoFilePath);
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

        private void CreateThumbnailSheet(CreateThumbnailSheetOptions options, VideoFileInformationEntity videoFileInformation)
        {
            var request = new ThumbnailSheetCreateRequest(
                videoPath:options.VideoFilePath,
                sheetName:Path.GetFileNameWithoutExtension(options.VideoFilePath),
                numberOfThumbnails:options.NumberOfThumbnails,
                videoDurationInSeconds:videoFileInformation.DurationInSecs);
            var response = _thumbnailSheetCreationService.Create(request);
            if (response.IsValid)
            {
                _auditor.AuditSuccess("VideoFile", options.VideoFilePath, "ThumbnailSheetCreated", response);
            }
            else
            {
                _auditor.AuditFailure("VideoFile", options.VideoFilePath, "ThumbnailSheetCreationFailed", response);
            }
        }

    }
}
