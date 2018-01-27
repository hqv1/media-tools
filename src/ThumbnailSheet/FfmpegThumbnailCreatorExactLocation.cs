using System;
using System.IO;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.Seedwork.App;
using Hqv.Seedwork.Exceptions;

namespace Hqv.MediaTools.ThumbnailSheet
{
    /// <summary>
    /// Uses FFmpeg to extract a thumbnail at an exact point in the video
    /// </summary>
    internal class FfmpegThumbnailCreatorExactLocation
    {
        private static readonly object Lock = new object();

        private readonly ThumbnailSheetCreationService.Config _config;

        public FfmpegThumbnailCreatorExactLocation(ThumbnailSheetCreationService.Config config)
        {
            _config = config;
        }

        public void CreateThumbnail(
            ThumbnailSheetCreateRequest request, 
            ThumbnailSheetCreationService.Response response,
            TimeSpan currentPointInVideo, 
            string outputFilepath)
        {
            var arguments = $"-i \"{request.VideoPath}\" -ss {currentPointInVideo} -vframes 1 {outputFilepath}";
            var app = new CommandLineApplication();
            var result = app.Run(_config.FfmpegPath, arguments);
            var errorInfo = result.ErrorData.Trim();            

            // Success means that the thumbnail is created.
            if (File.Exists(outputFilepath)) return;
            
            // lock is requried because we are accessing the response properties on more than one thread
            lock (Lock)
            {
                response.FfmpegArguments = arguments;
                response.FfmpegErrorOutput = errorInfo;
                throw new HqvException($"No thumbnail created using ffmpeg for {outputFilepath}");
            }
        }        
    }
}