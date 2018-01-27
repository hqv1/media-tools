using System;
using FluentAssertions;
using Hqv.MediaTools.Types.VideoFileInfo;
using Hqv.MediaTools.VideoFileInfo;
using Microsoft.Extensions.Options;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Hqv.VideoFileInfo.Tests
{
    /// <summary>
    /// Video File Info Service Integration Tests
    /// </summary>
    public class VideoFileInfoService_IntegrationTest
    {
        
        private const string FfprobePath = @"C:\Apps\ffmpeg\bin\ffprobe.exe";
        private const string VideoFilePathValid = @"C:\Workspace\media-tools-space\test-files\JLT.mp4";

        private VideoFileInfoExtractRequest _request;
        private readonly VideoFileInfoExtractionService _videoFileInfoExtractionService;
        private VideoFileInfoExtractResponse _response;

        public VideoFileInfoService_IntegrationTest()
        {                        
            var settings = new VideoFileInfoExtractionService.Config(FfprobePath);            
            _videoFileInfoExtractionService = new VideoFileInfoExtractionService(Options.Create(settings));
        }

        /// <summary>
        /// Given a video file, get information about that video file
        /// </summary>
        [Fact, Trait("Category", "Integration")]
        public void Should_ExtractVideoFileInformation()
        {
            GivenAVideoFile();
            WhenTheServiceIsCalled();
            ThenVideoFileInformationIsExtracted();
        }        

        private void GivenAVideoFile()
        {
            _request = new VideoFileInfoExtractRequest(VideoFilePathValid, Guid.NewGuid().ToString());
        }

        private void WhenTheServiceIsCalled()
        {
            _response = _videoFileInfoExtractionService.Extract(_request);
        }

        private void ThenVideoFileInformationIsExtracted()
        {
            _response.VideoFileInformation.Should().NotBeNull();
        }
    }
}
