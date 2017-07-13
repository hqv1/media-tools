using FluentAssertions;
using Hqv.MediaTools.Types.Thumbnail;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Hqv.MediaTools.Thumbnail.Tests
{
    public class ThumbnailCreationNotAccurateService_IntegrationTest
    {
        
        private const string FfmpegPath = @"C:\Apps\ffmpeg\bin\ffmpeg.exe";
        private const int GetThumbnailEveryNSeconds = 10;
        private const string TempThumbnailPath = @"C:\Temp\TempThumbnailPath";
        private const string VideoPath = @"C:\Temp\Black_Panther_Teaser_Trailer.webm";
        private readonly ThumbnailCreationNotAccurateService _service;
        private ThumbnailCreationRequest _request;

        public ThumbnailCreationNotAccurateService_IntegrationTest()
        {
            var settings = new ThumbnailCreationNotAccurateService.Settings(TempThumbnailPath, FfmpegPath);
            _service = new ThumbnailCreationNotAccurateService(settings);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void Should_CreateThumbnails()
        {
            _request = new ThumbnailCreationRequest(VideoPath, GetThumbnailEveryNSeconds);
            var response = _service.Create(_request);
            response.IsValid.Should().BeTrue();
        }
    }
}