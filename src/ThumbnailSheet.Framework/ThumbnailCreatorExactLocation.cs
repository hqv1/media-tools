using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hqv.MediaTools.Types.ThumbnailSheet;

namespace Hqv.MediaTools.ThumbnailSheet.Framework
{
    /// <summary>
    /// Create thumbnails at specific points in the video.
    /// 
    /// THere's a few ways we can get thumbnails using FFmpeg. 
    /// See here https://trac.ffmpeg.org/wiki/Create%20a%20thumbnail%20image%20every%20X%20seconds%20of%20the%20video.
    /// My issues is that it tends to not make the expected number of thumbnails and not at the time I'm expecting. 
    /// Also if I wanted to add a timestamp, I'm only guessing at the thumbnail timestamp with using the previous method.
    /// 
    /// The downside of this method is that it's slow. Really slow. Using Parallel.ForEach to try to speed it up.
    /// 
    /// </summary>
    internal class ThumbnailCreatorExactLocation
    {
        private class ThumbnailInfo
        {
            public ThumbnailInfo(string outputFilepath, TimeSpan pointInVideo)
            {
                OutputFilepath = outputFilepath;
                PointInVideo = pointInVideo;

            }

            public string OutputFilepath { get; }
            public TimeSpan PointInVideo { get; }
        }

        private readonly ThumbnailSheetCreationService.Settings _settings;
        private ThumbnailSheetCreateRequest _request;
        private ThumbnailSheetCreationService.Response _response;
        private readonly TimeStamper _timeStamper;

        public ThumbnailCreatorExactLocation(ThumbnailSheetCreationService.Settings settings)
        {
            _settings = settings;
            _timeStamper = new TimeStamper();
        }

        public void CreateThumbnails(ThumbnailSheetCreateRequest request, ThumbnailSheetCreationService.Response response)
        {
            _request = request;
            _response = response;
            var secondPerThumbnail = request.VideoDurationInSeconds / request.NumberOfThumbnails;
            var timespanToAdd = TimeSpan.FromSeconds(secondPerThumbnail);
            // start at the halfway point from beginning and timespan to add. Should give us good thumbnails
            var currentPointInVideo = TimeSpan.FromSeconds(secondPerThumbnail / 2); 

            var thumbnailsSettings = new List<ThumbnailInfo>();
            for (var currentThumbnailNumber = 1; currentThumbnailNumber <= request.NumberOfThumbnails; ++currentThumbnailNumber)
            {
                var outputFilepath = $"{_settings.TempThumbnailPath}\\thumbnail-{currentThumbnailNumber:D3}.png";
                thumbnailsSettings.Add(new ThumbnailInfo(outputFilepath, currentPointInVideo));
                currentPointInVideo = currentPointInVideo.Add(timespanToAdd);
            }
                                 
            Parallel.ForEach(thumbnailsSettings, thumbnailsSetting =>
            {
                var tc = new FfmpegThumbnailCreatorExactLocation(_settings);
                tc.CreateThumbnail(_request, _response, thumbnailsSetting.PointInVideo, thumbnailsSetting.OutputFilepath);                
            });

            if (request.ShouldAddTimestamps)
            {
                AddTimeStamps(request, thumbnailsSettings);
            }
        }

        private void AddTimeStamps(ThumbnailSheetCreateRequest request, List<ThumbnailInfo> thumbnailsSettings)
        {
            Parallel.ForEach(thumbnailsSettings, thumbnailsSetting =>
            {
                _timeStamper.Stamp(request, thumbnailsSetting.OutputFilepath, thumbnailsSetting.PointInVideo);
            });
        }
    }
}