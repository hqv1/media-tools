using System;
using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using Hqv.CSharp.Common.App;
using Hqv.CSharp.Common.Exceptions;
using Hqv.CSharp.Common.Validations;
using Hqv.MediaTools.Types.FileDownload;

namespace Hqv.MediaTools.FileDownload
{    
    /// <summary>
    /// Download a file using ffmpeg
    /// </summary>
    public class FileDownloaderService : IFileDownloaderService
    {
        private CommandLineApplicationAsync _commandLineApplicationAsync;
        private DownloadRequest _request;
        private Response _response;
        private readonly Settings _settings;                

        public class Settings
        {
            public Settings(string ffmpegPath, string savePath)
            {
                FfmpegPath = ffmpegPath;
                SavePath = savePath;

                Validator.Validate<Settings, SettingsValidator>(this);
            }

            /// <summary>
            /// Save directory
            /// </summary>
            public string SavePath { get; }
            /// <summary>
            /// FFmpeg path
            /// </summary>
            public string FfmpegPath { get; }            
        }

        public class SettingsValidator : AbstractValidator<Settings>
        {
            public SettingsValidator()
            {
                RuleFor(x => x.SavePath).Must(Directory.Exists);
                //RuleFor(x => x.FfmpegPath).Must(File.Exists);
            }
        }

        public class Response : DownloadResponse
        {
            public Response(DownloadRequest request) : base(request)
            {
            }

            /// <summary>
            /// FfmpegArguments. Only populated on error
            /// </summary>
            public string FfmpegArguments { get; set; }

            public string StandardOutput { get; set; }
            public string StandardError { get; set; }
            public int ResultCode { get; set; }
        }

        public FileDownloaderService(Settings settings)
        {
            _settings = settings;
        }

        public async Task<DownloadResponse> Download(DownloadRequest request)
        {
            _request = request;
            _response = new Response(request);
            try
            {
                Validator.Validate<DownloadRequest, DownloadRequestValidator>(_request);
                await DownloadTry();
            }
            catch (HqvException ex)
            {
                _response.AddError(ex);
            }
            catch (Exception ex)
            {                
                _response.AddError(new Exception("Unhandled exception in FileDownloaderService", ex));
            }
            return _response;
        }
        
        private async Task DownloadTry()
        {
            var extension = _request.OutputExtension;
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
            var outputFilePath = Path.Combine(_settings.SavePath, _request.OutputFileName + extension);            
            _response.FfmpegArguments = $"-i {_request.Url} -c copy {outputFilePath}";
            
            _commandLineApplicationAsync = new CommandLineApplicationAsync();
            var result = await _commandLineApplicationAsync.RunAsync(
                _settings.FfmpegPath,
                _response.FfmpegArguments,
                CreateProgress(),
                _request.CancellationToken);
           
            _response.StandardError = result.ErrorData;
            _response.StandardOutput = result.OutputData;
            _response.ResultCode = result.ResultCode;

            if (_response.ResultCode != 0)
            {
                throw new HqvException("Application result code is not successful");
            }
        }

        private Progress<ProcessProgress> CreateProgress()
        {
            Progress<ProcessProgress> progress = null;
            if (_request.Progress != null)
            {
                progress = new Progress<ProcessProgress>(p =>
                {
                    if (!string.IsNullOrEmpty(p.ErrorData))
                    {
                        _request.Progress.Report(p.ErrorData);
                    }
                    if (!string.IsNullOrEmpty(p.OutputData))
                    {
                        _request.Progress.Report(p.OutputData);
                    }
                });
            }
            return progress;
        }
    }    
}
