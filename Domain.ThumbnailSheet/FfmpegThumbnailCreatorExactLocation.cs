using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Hqv.CSharp.Common.Exceptions;
using Hqv.MediaTools.Domain;

namespace Domain.ThumbnailSheet
{
    internal class FfmpegThumbnailCreatorExactLocation
    {
        private static readonly object Lock = new object();

        private readonly ThumbnailSheetService.Settings _settings;
        private readonly StringBuilder _errorBuilder = new StringBuilder();
        private readonly StringBuilder _outputBuilder = new StringBuilder();        

        public FfmpegThumbnailCreatorExactLocation(ThumbnailSheetService.Settings settings)
        {
            _settings = settings;
        }

        public void CreateThumbnail(ThumbnailSheetCreateRequest request, ThumbnailSheetService.Response response,
            TimeSpan currentPointInVideo, string outputFilepath)
        {
            var arguments = $"-i \"{request.VideoPath}\" -ss {currentPointInVideo} -vframes 1 {outputFilepath}";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _settings.FfmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            process.OutputDataReceived += OutputHandler;
            process.ErrorDataReceived += ErrorHandler;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            var errorInfo = _errorBuilder.ToString().Trim();

            if (File.Exists(outputFilepath)) return;

            //race condition on response. Let the first error come in and throw the exception
            lock (Lock)
            {
                response.FfmpegArguments = arguments;
                response.FfmpegErrorOutput = errorInfo;
                throw new HqvException("No thumbnails created using ffmpeg");
            }            
        }

        private void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            _outputBuilder.AppendLine(e.Data);
        }

        private void ErrorHandler(object sender, DataReceivedEventArgs e)
        {
            _errorBuilder.AppendLine(e.Data);
        }
    }
}