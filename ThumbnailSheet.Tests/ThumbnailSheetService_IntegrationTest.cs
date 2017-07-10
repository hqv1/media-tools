using System.IO;
using System.Linq;
using FluentAssertions;
using Hqv.MediaTools.ThumbnailSheet;
using Hqv.MediaTools.Types;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Hqv.ThumbnailSheet.Tests
{
    public class ThumbnailSheetService_IntegrationTest
    {
        private const string tempThumbnailPath = @"C:\Temp\TempThumbnailPath";
        private const string thumbnailSheetPath = @"C:\Temp\ThumbnailSheetPath";
        private const string ffmpegPath = @"C:\Apps\ffmpeg\bin\ffmpeg.exe";

        private const string outputFilePath = @"C:\Temp\ThumbnailSheetPath\Black_Panther_Teaser_Trailer-sheet.jpg";

        private readonly ThumbnailSheetService _service;
        private ThumbnailSheetCreateRequest _request;
        private ThumbnailSheetCreateResponse _response;

        public ThumbnailSheetService_IntegrationTest()
        {            
            var settings = new ThumbnailSheetService.Settings(tempThumbnailPath, thumbnailSheetPath, ffmpegPath);
            _service = new ThumbnailSheetService(settings);

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
                videoPath: @"C:\Temp\Black_Panther_Teaser_Trailer.webm",
                sheetName: "Black_Panther_Teaser_Trailer",
                numberOfThumbnails: 16,
                videoDurationInSeconds: 112,
                sheetQuality: 80,
                sheetTitleFontSize: 12,
                thumbnailWidth: 160);
        }

        private void GivenAInvalidRequest()
        {
            _request = new ThumbnailSheetCreateRequest(
                videoPath: @"C:\Temp\Black_Panther_Teaser_Trailer.webm",
                sheetName: "Black_Panther_Teaser_Trailer",
                numberOfThumbnails: 9,
                videoDurationInSeconds: 112
,
                sheetQuality: -2,
                sheetTitleFontSize: 12,
                thumbnailWidth: 180);
        }

        private void WhenTheServiceIsCalled()
        {
            _response = _service.Create(_request);
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
