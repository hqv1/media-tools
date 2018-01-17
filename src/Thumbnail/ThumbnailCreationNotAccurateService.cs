using System;
using System.IO;
using System.Linq;
using FluentValidation;
using Hqv.MediaTools.Types.Thumbnail;
using Hqv.Seedwork.App;
using Hqv.Seedwork.Exceptions;
using Hqv.Seedwork.Validations;

namespace Hqv.MediaTools.Thumbnail
{

    /// <summary>
    /// Based on https://trac.ffmpeg.org/wiki/Create%20a%20thumbnail%20image%20every%20X%20seconds%20of%20the%20video
    /// 
    /// Using FFmpeg to create the thumbnail. Using the argument -vf quickly creates thumbnails but doesn't seem to be
    /// in the expected timeframe. This is a quick and dirty way of getting thumbnails.
    /// 
    /// The more accurate way is to create a list of specific times in the video to get the thumbnails. And then 
    /// request FFmpeg to go there and create the thumbnail. That's how ThumbnailSheet creates thumbnails. But it's
    /// really slow.
    /// </summary>
    public class ThumbnailCreationNotAccurateService : IThumbnailCreationService
    {
        private readonly Settings _settings;
        private ThumbnailCreationRequest _request;
        private Response _response;

        public class Settings
        {
            public Settings(string thumbnailPath, string ffmpegPath)
            {
                ThumbnailPath = thumbnailPath;
                FfmpegPath = ffmpegPath;

                Validator.Validate<Settings, SettingsValidator>(this);
            }
            public string ThumbnailPath { get; }
            /// <summary>
            /// FFmpeg path
            /// </summary>
            public string FfmpegPath { get; }            
        }

        public class SettingsValidator : AbstractValidator<Settings>
        {
            public SettingsValidator()
            {
                RuleFor(x => x.ThumbnailPath).Must(Directory.Exists);
                RuleFor(x => x.FfmpegPath).Must(File.Exists);                
            }
        }

        public class Response : ThumbnailCreateResponse
        {
            public Response(ThumbnailCreationRequest request) : base(request)
            {
            }

            /// <summary>
            /// FfmpegArguments. Only populated on error
            /// </summary>
            public string FfmpegArguments { get; set; }
            /// <summary>
            /// FfmpegErrorOutput. Only populated on error
            /// </summary>
            public string FfmpegErrorOutput { get; set; }
        }
        
        public ThumbnailCreationNotAccurateService(Settings settings)
        {
            _settings = settings;
        }

        public ThumbnailCreateResponse Create(ThumbnailCreationRequest request)
        {
            _request = request;
            _response = new Response(request);
            try
            {
                CreateTry();
            }
            catch (HqvException ex)
            {
                _response.AddError(ex);
            }
            catch (Exception ex)
            {
                const string message = "Unhandled exception in ThumbnailCreationNotAccurateService";
                _response.AddError(new Exception(message, ex));
            }
            return _response;
        }

        private void CreateTry()
        {
            CleanupPreviousFiles();
            var filename = Path.GetFileNameWithoutExtension(_request.VideoPath);
            var outputPath = Path.Combine(_settings.ThumbnailPath, filename + "-%03d.jpg");
            var arguments = $"-i \"{_request.VideoPath}\" -vf fps=1/{_request.GetThumbnailEveryNSeconds} \"{outputPath}\"";
            _response.FfmpegArguments = arguments;

            var app = new CommandLineApplication();
            var commandLineResult = app.Run(_settings.FfmpegPath, arguments);
            _response.FfmpegErrorOutput = commandLineResult.ErrorData;

            if (AnyCreatedFile()) return;

            throw new HqvException("No thumbnails created. See response for more information");
        }

        private void CleanupPreviousFiles()
        {
            var filename = Path.GetFileNameWithoutExtension(_request.VideoPath);            
            var previousFiles = Directory.GetFiles(_settings.ThumbnailPath, filename+ "*.jpg");
            foreach (var file in previousFiles)
            {
                File.Delete(file);
            }
        }

        private bool AnyCreatedFile()
        {
            var filename = Path.GetFileNameWithoutExtension(_request.VideoPath);
            return Directory.GetFiles(_settings.ThumbnailPath, filename + "*.jpg").Any();
        }
    }
}
