using System;
using System.IO;
using FluentValidation;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.Seedwork.Exceptions;

namespace Hqv.MediaTools.ThumbnailSheet
{       
    /// <summary>
    /// Thumbnail sheet service
    /// 
    /// todo: validation can be abstracted to a shared library
    /// </summary>
    public class ThumbnailSheetCreationService : IThumbnailSheetCreationService
    {
        private readonly Settings _settings;
        private readonly SheetCreator _sheetCreator;
        private readonly ThumbnailCreatorExactLocation _thumbnailCreator;        

        private Response _response;

        /// <summary>
        /// Settings for the service
        /// </summary>
        public class Settings
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="tempThumbnailPath">Temporary directory to store thumbnails</param>
            /// <param name="thumbnailSheetPath">Directory to save the thumbnail</param>
            /// <param name="ffmpegPath">FFmpeg path</param>
            public Settings(string tempThumbnailPath, string thumbnailSheetPath, string ffmpegPath)
            {
                TempThumbnailPath = tempThumbnailPath;
                ThumbnailSheetPath = thumbnailSheetPath;
                FfmpegPath = ffmpegPath;

                Validate();
            }

            /// <summary>
            /// Temporary directory to store thumbnails. Files will be deleted once done. Don't store files you may want
            /// in this directory
            /// </summary>
            public string TempThumbnailPath { get; }
            /// <summary>
            /// Directory to save the thumbnail
            /// </summary>
            public string ThumbnailSheetPath { get; }
            /// <summary>
            /// FFmpeg path
            /// </summary>
            public string FfmpegPath { get; }
            
            private void Validate()
            {
                var validator = new SettingsValidator();
                var validationResult = validator.Validate(this);
                if (validationResult.IsValid) return;

                var exception = new HqvException("Validation failed");
                exception.Data["errors"] = validationResult.Errors;
                throw exception;
            }
        }

        /// <summary>
        /// Validate Settings
        /// </summary>
        private class SettingsValidator : AbstractValidator<Settings>
        {
            public SettingsValidator()
            {
                RuleFor(x => x.TempThumbnailPath).Must(Directory.Exists);
                RuleFor(x => x.ThumbnailSheetPath).Must(Directory.Exists);
                RuleFor(x => x.FfmpegPath).Must(File.Exists);
            }
        }

        /// <summary>
        /// Add some additional information to the response
        /// </summary>
        public class Response : ThumbnailSheetCreateResponse
        {
            public Response(ThumbnailSheetCreateRequest request) : base(request)
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Settings</param>
        public ThumbnailSheetCreationService(Settings settings)
        {
            _settings = settings;

            _thumbnailCreator = new ThumbnailCreatorExactLocation(_settings);
            _sheetCreator = new SheetCreator(_settings);
        }

        /// <summary>
        /// Create a thumbnail sheet
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        public ThumbnailSheetCreateResponse Create(ThumbnailSheetCreateRequest request)
        {            
            _response = new Response(request);
            try
            {
                CreateTry(request);
            }
            catch (HqvException ex)
            {
                _response.AddError(ex);
            }
            catch (Exception ex)
            {
                const string message = "Unhandled exception in ThumbnailSheetCreationService";                
                _response.AddError(new Exception(message, ex));
            }            
            return _response;
        }

        private void CreateTry(ThumbnailSheetCreateRequest request)
        {
            ValidateRequest(request);
            CleanupTempFolder();
            _thumbnailCreator.CreateThumbnails(request, _response);
            _sheetCreator.CreateSheet(request,_response);
            CleanupTempFolder();
        }

        private static void ValidateRequest(ThumbnailSheetCreateRequest request)
        {
            var validator = new ThumbnailSheetCreateRequestValidator();
            var validationResult = validator.Validate(request);
            if (validationResult.IsValid) return;

            var exception = new HqvException("Validation on request failed");
            exception.Data["errors"] = validationResult.Errors;
            throw exception;
        }

        /// <summary>
        /// Delete files from the temp folder
        /// </summary>
        private void CleanupTempFolder()
        {
            foreach (var file in Directory.GetFiles(_settings.TempThumbnailPath, "thumbnail*.png"))
            {
                File.Delete(file);
            }
        }        
    }    
}
