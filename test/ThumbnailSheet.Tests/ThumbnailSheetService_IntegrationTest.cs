using System.IO;
using System.Linq;
using FluentAssertions;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Microsoft.Extensions.Options;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Hqv.MediaTools.ThumbnailSheet.Tests
{
    public class ThumbnailSheetService_IntegrationTest
    {
        private const string tempThumbnailPath = @"C:\Workspace\media-tools-space\thumbnails";
        private const string thumbnailSheetPath = @"C:\Workspace\media-tools-space\thumbnails-sheet";
        private const string ffmpegPath = @"C:\Apps\ffmpeg\bin\ffmpeg.exe";

        private const string outputFilePath = @"C:\Workspace\media-tools-space\thumbnails-sheet\JLT-sheet.jpg";

        private readonly ThumbnailSheetCreationService _creationService;
        private ThumbnailSheetCreateRequest _request;
        private ThumbnailSheetCreateResponse _response;

        public ThumbnailSheetService_IntegrationTest()
        {            
            var settings = new ThumbnailSheetCreationService.Config(tempThumbnailPath, thumbnailSheetPath, ffmpegPath);
            _creationService = new ThumbnailSheetCreationService(Options.Create(settings));

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }

        [Fact]
        [Trait("Category","Integration")]
        public void Should_CreateThumbnailSheet()
        {
            GivenAValidRequest();
            WhenTheServiceIsCalled();
            ThenNoErrorsOccurred();
            ThenAThumbnailSheetIsCreated();
        }       

        [Fact]
        [Trait("Category", "Integration")]
        public void Should_ReturnError_IfRequestHasNegativeSheetQuality()
        {
            GivenAInvalidRequest();
            WhenTheServiceIsCalled();

            _response.IsValid.Should().BeFalse();
            _response.Errors.Any(x => x.Message.ToLower().Contains("validation")).Should().BeTrue();
        }

        private void GivenAValidRequest()
        {
            _request = new ThumbnailSheetCreateRequest(
                videoPath: @"C:\Workspace\media-tools-space\test-files\JLT.mp4",
                sheetName: "JLT",
                numberOfThumbnails: 16,
                videoDurationInSeconds: 152,
                sheetQuality: 80,
                sheetTitleFontSize: 12,
                thumbnailWidth: 320);
        }

        private void GivenAInvalidRequest()
        {
            _request = new ThumbnailSheetCreateRequest(
                videoPath: @"C:\Workspace\media-tools-space\test-files\INVALID-FILE.mp4",
                sheetName: "JLT",
                numberOfThumbnails: 9,
                videoDurationInSeconds: 152
,
                sheetQuality: -2,
                sheetTitleFontSize: 12,
                thumbnailWidth: 180);
        }

        private void WhenTheServiceIsCalled()
        {
            _response = _creationService.Create(_request);
        }

        private void ThenNoErrorsOccurred()
        {
            _response.IsValid.Should().BeTrue();
        }

        private void ThenAThumbnailSheetIsCreated()
        {
            _response.SheetFilePath.ToLower().Should().Be(outputFilePath.ToLower());
            File.Exists(_response.SheetFilePath);
        }
    }
}
