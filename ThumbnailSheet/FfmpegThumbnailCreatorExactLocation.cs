using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Hqv.CSharp.Common.Exceptions;
using Hqv.MediaTools.Types.ThumbnailSheet;

namespace Hqv.MediaTools.ThumbnailSheet
{
    /// <summary>
    /// Uses FFmpeg to extract a thumbnail at an exact point in the video
    /// </summary>
    internal class FfmpegThumbnailCreatorExactLocation
    {
        private static readonly object Lock = new object();

        private readonly ThumbnailSheetCreationService.Settings _settings;
        private readonly StringBuilder _errorBuilder = new StringBuilder();
        private readonly StringBuilder _outputBuilder = new StringBuilder();        

        public FfmpegThumbnailCreatorExactLocation(ThumbnailSheetCreationService.Settings settings)
        {
            _settings = settings;
        }

        public void CreateThumbnail(
            ThumbnailSheetCreateRequest request, 
            ThumbnailSheetCreationService.Response response,
            TimeSpan currentPointInVideo, 
            string outputFilepath)
        {
            var arguments = $"-i \"{request.VideoPath}\" -ss {currentPointInVideo} -vframes 1 {outputFilepath}";

            var process = new Process
            {
                StartInfo = CreateProcessStartInfo(arguments)
            };
            process.OutputDataReceived += OutputHandler;
            process.ErrorDataReceived += ErrorHandler;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            // All outputs for FFmpeg goes to the error stream.
            var errorInfo = _errorBuilder.ToString().Trim();

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

        private ProcessStartInfo CreateProcessStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = _settings.FfmpegPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
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