using System;
using System.IO;
using System.Threading.Tasks;
using Hqv.MediaTools.Types.FileDownload;
using Hqv.Seedwork.App;
using Hqv.Seedwork.Exceptions;
using Hqv.Seedwork.Validations;
using Microsoft.Extensions.Options;

namespace Hqv.MediaTools.FileDownload
{    
    /// <inheritdoc />
    /// <summary>
    /// Download a file using ffmpeg
    /// </summary>
    public class FileDownloaderService : IFileDownloaderService
    {
        private CommandLineApplicationAsync _commandLineApplicationAsync;
        private DownloadRequest _request;
        private Response _response;
        private readonly Config _config;                

        public class Config
        {
            public const string ConfigurationSectionName = nameof(FileDownloaderService);

            public Config()
            {
                
            }

            public Config(string ffmpegPath, string savePath)
            {
                FfmpegPath = ffmpegPath;
                SavePath = savePath;               
            }

            /// <summary>
            /// Save directory
            /// </summary>
            public string SavePath { get; set; }
            /// <summary>
            /// FFmpeg path
            /// </summary>
            public string FfmpegPath { get; set; }            
        }
               
        public FileDownloaderService(IOptions<Config>  config)
        {
            _config = config.Value;
            Validator.Validate<Config, ConfigValidator>(_config);
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
            var outputFilePath = Path.Combine(_config.SavePath, _request.OutputFileName + extension);            
            _response.FfmpegArguments = $"-i {_request.Url} -c copy {outputFilePath}";
            
            _commandLineApplicationAsync = new CommandLineApplicationAsync();
            var result = await _commandLineApplicationAsync.RunAsync(
                _config.FfmpegPath,
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
