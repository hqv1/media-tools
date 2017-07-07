﻿using System;
using FluentAssertions;
using Hqv.MediaTools.Domain;
using MediaTools.Domain.VideoFileInfo;
using Xunit;

namespace MediaTools.Domain.VideoFileInfoTests
{
    public class VideoFileInfoServiceTest
    {
        // Path to FFProbe
        private const string FfprobePath = @"C:\Apps\ffmpeg\bin\ffprobe.exe";
        // Video file to extract
        private const string VideoFilePathValid = @"C:\Temp\Black_Panther_Teaser_Trailer.webm";

        private VideoFileInfoRequest _request;
        private readonly VideoFileInfoService _videoFileInfoService;
        private VideoFileInfoResponse _response;


        public VideoFileInfoServiceTest()
        {            
            var settings = new VideoFileInfoService.Settings(FfprobePath);            
            _videoFileInfoService = new VideoFileInfoService(settings);
        }

        [Fact, Trait("Category", "Integration")]
        public void Should_ExtractVideoFileInformation()
        {
            GivenAVideoFile();
            WhenTheServiceIsCalled();
            ThenVideoFileInformationIsExtracted();
        }

        private void GivenAVideoFile()
        {
            _request = new VideoFileInfoRequest(VideoFilePathValid);
        }

        private void WhenTheServiceIsCalled()
        {
            _response = _videoFileInfoService.Extract(_request);
        }

        private void ThenVideoFileInformationIsExtracted()
        {
            _response.VideoFileInformation.Should().NotBeNull();
        }
    }
}
