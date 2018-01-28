using System;
using System.IO;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.Seedwork.Exceptions;
using Hqv.Seedwork.Validations;
using Microsoft.Extensions.Options;

namespace Hqv.MediaTools.ThumbnailSheet
{
    /// <inheritdoc />
    /// <summary>
    /// Thumbnail sheet service
    /// todo: validation can be abstracted to a shared library
    /// </summary>
    public class ThumbnailSheetCreationService : IThumbnailSheetCreationService
    {
        private readonly Config _config;
        private readonly SheetCreator _sheetCreator;
        private readonly ThumbnailCreatorExactLocation _thumbnailCreator;        

        private Response _response;
        
        public class Config
        {
            public const string ConfigurationSectionName = nameof(ThumbnailSheetCreationService);

            public Config()
            {
                
            }
            
            public Config(string tempThumbnailPath, string thumbnailSheetPath, string ffmpegPath)
            {
                TempThumbnailPath = tempThumbnailPath;
                ThumbnailSheetPath = thumbnailSheetPath;
                FfmpegPath = ffmpegPath;                
            }

            /// <summary>
            /// Temporary directory to store thumbnails. Files will be deleted once done. Don't store files you may want
            /// in this directory
            /// </summary>
            public string TempThumbnailPath { get; set; }
            /// <summary>
            /// Directory to save the thumbnail
            /// </summary>
            public string ThumbnailSheetPath { get; set; }
            /// <summary>
            /// FFmpeg path
            /// </summary>
            public string FfmpegPath { get; set; }            
        }
               
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Config</param>
        public ThumbnailSheetCreationService(IOptions<Config> config)
        {
            _config = config.Value;
            Validator.Validate<Config, ConfigValidator>(_config);

            _thumbnailCreator = new ThumbnailCreatorExactLocation(_config);
            _sheetCreator = new SheetCreator(_config);
        }

        /// <inheritdoc />
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
            foreach (var file in Directory.GetFiles(_config.TempThumbnailPath, "thumbnail*.png"))
                File.Delete(file);
        }        
    }
}
