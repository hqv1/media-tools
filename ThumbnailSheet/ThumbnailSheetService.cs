using System;
using System.IO;
using FluentValidation;
using Hqv.CSharp.Common.Exceptions;
using Hqv.MediaTools.Types.ThumbnailSheet;

namespace Hqv.MediaTools.ThumbnailSheet
{       
    /// <summary>
    /// Thumbnail sheet service
    /// </summary>
    public class ThumbnailSheetService : IThumbnailSheetService
    {
        private readonly Settings _settings;
        private Response _response;
        private readonly ThumbnailCreatorExactLocation _thumbnailCreator;
        private readonly SheetCreator _sheetCreator;

        public class Settings
        {
            public Settings(string tempThumbnailPath, string thumbnailSheetPath, string ffmpegPath)
            {
                TempThumbnailPath = tempThumbnailPath;
                ThumbnailSheetPath = thumbnailSheetPath;
                FfmpegPath = ffmpegPath;

                Validate();
            }

            public string TempThumbnailPath { get; }
            public string ThumbnailSheetPath { get; }
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

        private class SettingsValidator : AbstractValidator<Settings>
        {
            public SettingsValidator()
            {
                RuleFor(x => x.TempThumbnailPath).Must(Directory.Exists);
                RuleFor(x => x.ThumbnailSheetPath).Must(Directory.Exists);
                RuleFor(x => x.FfmpegPath).Must(File.Exists);
            }
        }

        public class Response : ThumbnailSheetCreateResponse
        {
            public Response(ThumbnailSheetCreateRequest request) : base(request)
            {
            }

            public string FfmpegArguments { get; set; }
            public string FfmpegErrorOutput { get; set; }
        }

        public ThumbnailSheetService(Settings settings)
        {
            _settings = settings;

            _thumbnailCreator = new ThumbnailCreatorExactLocation(_settings);
            _sheetCreator = new SheetCreator(_settings);
        }

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
                const string message = "Unhandled exception in ThumbnailSheetService";                
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
            if (!validationResult.IsValid)
            {
                var exception = new HqvException("Validation on request failed");
                exception.Data["errors"] = validationResult.Errors;
                throw exception;
            }
        }

        private void CleanupTempFolder()
        {
            foreach (var file in Directory.GetFiles(_settings.TempThumbnailPath, "thumbnail*.png"))
            {
                File.Delete(file);
            }
        }        
    }


    
}
