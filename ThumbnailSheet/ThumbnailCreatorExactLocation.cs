using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hqv.MediaTools.Types.ThumbnailSheet;

namespace Hqv.MediaTools.ThumbnailSheet
{
    internal class ThumbnailCreatorExactLocation
    {
        private class ThumbnailSettings
        {
            public ThumbnailSettings(string outputFilepath, TimeSpan pointInVideo)
            {
                OutputFilepath = outputFilepath;
                PointInVideo = pointInVideo;

            }

            public string OutputFilepath { get; }
            public TimeSpan PointInVideo { get; }
        }

        private readonly ThumbnailSheetService.Settings _settings;
        private ThumbnailSheetCreateRequest _request;
        private ThumbnailSheetService.Response _response;
        private readonly TimeStamper _timeStamper;

        public ThumbnailCreatorExactLocation(ThumbnailSheetService.Settings settings)
        {
            _settings = settings;

            _timeStamper = new TimeStamper(_settings);
        }

        public void CreateThumbnails(ThumbnailSheetCreateRequest request, ThumbnailSheetService.Response response)
        {
            _request = request;
            _response = response;
            var secondPerThumbnail = request.VideoDurationInSeconds / request.NumberOfThumbnails;
            var timespanToAdd = TimeSpan.FromSeconds(secondPerThumbnail);
            var currentPointInVideo = TimeSpan.FromSeconds(secondPerThumbnail / 2);

            var thumbnailsSettings = new List<ThumbnailSettings>();
            for (var currentThumbnailNumber = 1; currentThumbnailNumber <= request.NumberOfThumbnails; ++currentThumbnailNumber)
            {
                var outputFilepath = $"{_settings.TempThumbnailPath}\\thumbnail-{currentThumbnailNumber:D3}.png";
                thumbnailsSettings.Add(new ThumbnailSettings(outputFilepath, currentPointInVideo));
                currentPointInVideo = currentPointInVideo.Add(timespanToAdd);
            }
                                 
            Parallel.ForEach(thumbnailsSettings, thumbnailsSetting =>
            {
                var tc = new FfmpegThumbnailCreatorExactLocation(_settings);
                tc.CreateThumbnail(_request, _response, thumbnailsSetting.PointInVideo, thumbnailsSetting.OutputFilepath);
                
            });

            if (request.ShouldAddTimestamps)
            {
                Parallel.ForEach(thumbnailsSettings, thumbnailsSetting =>
                {
                    _timeStamper.Stamp(request, thumbnailsSetting.OutputFilepath, thumbnailsSetting.PointInVideo);
                });
            }
        }        
    }
}