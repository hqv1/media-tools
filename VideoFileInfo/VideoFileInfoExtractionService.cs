﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Hqv.CSharp.Common.Exceptions;
using Hqv.MediaTools.Types.VideoFileInfo;
using Newtonsoft.Json.Linq;

namespace Hqv.MediaTools.VideoFileInfo
{
    /// <summary>
    /// Video File Info Extraction service.
    /// 
    /// It uses FFprobe to get the information. It runs FFprobe as a command line with arguments, 
    /// gets its output as a JSON and parses the output to obtain the Video File information.
    /// 
    /// You can get FFProbe at http://ffmpeg.org/download.html. It's part of the FFMpeg download.
    /// Known to work with version 3.3.2.
    /// 
    /// todo: Creating and running a process could probably be put into a shared library 
    /// </summary>
    public class VideoFileInfoExtractionService : IVideoFileInfoExtractionService
    {
        private readonly Settings _settings;
        private readonly FfprobeResultParser _ffprobeResultParser;
        private Response _response;

        private readonly StringBuilder _errorBuilder = new StringBuilder();
        private readonly StringBuilder _outputBuilder = new StringBuilder();        

        /// <summary>
        /// Settings for the service
        /// </summary>
        public class Settings
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="ffprobePath"> FFProbe path.</param>
            public Settings(string ffprobePath)
            {
                FfprobePath = ffprobePath;
                Validate();
            }

            private void Validate()
            {
                if (!File.Exists(FfprobePath))
                {
                    throw new HqvException($"{FfprobePath} does not exist. Must be a valid location to ffprobe");
                }
            }

            /// <summary>
            /// FFProbe path
            /// </summary>
            public string FfprobePath { get; }
        }

        /// <summary>
        /// Return additional information than just the generic response
        /// </summary>
        public class Response : VideoFileInfoExtractResponse
        {
            public Response(VideoFileInfoExtractRequest request) : base(request)
            {
            }

            /// <summary>
            /// FFProbe arguments
            /// </summary>
            public string FfprobeArguments { get; set; }

            /// <summary>
            /// Output from FFProbe
            /// </summary>
            public string FfprobeOutput { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Settings</param>
        public VideoFileInfoExtractionService(Settings settings)
        {
            _settings = settings;

            _ffprobeResultParser = new FfprobeResultParser();
        }

        /// <summary>
        /// Extract video information
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        public VideoFileInfoExtractResponse Extract(VideoFileInfoExtractRequest request)
        {
            _response = new Response(request);
            try
            {
                ExtractTry(request);
            }
            catch (HqvException ex)
            {
                _response.AddError(ex);
            }
            catch (Exception ex)
            {
                const string message = "Unhandled exception in VideoFileInfoExtractionService";                
                _response.AddError(new Exception(message, ex));
            }
            return _response;
        }

        private void ExtractTry(VideoFileInfoExtractRequest request)
        {
            RunFfprobe(request);
            var json = GetJsonFromFfprobeResult();
            _response.VideoFileInformation = _ffprobeResultParser.Parse(json);
        }

        private void RunFfprobe(VideoFileInfoExtractRequest request)
        {
            var arguments = $"-v quiet -print_format json -show_format -show_streams \"{request.VideoFilePath}\"";
            _response.FfprobeArguments = arguments;
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
        }

        private ProcessStartInfo CreateProcessStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = _settings.FfprobePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private void ErrorHandler(object sender, DataReceivedEventArgs e)
        {
            _errorBuilder.AppendLine(e.Data);
        }

        private void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            _outputBuilder.AppendLine(e.Data);
        }        

        private JObject GetJsonFromFfprobeResult()
        {
            var errorInfo = _errorBuilder.ToString().Trim();
            var outputInfo = _outputBuilder.ToString().Trim();
            _response.FfprobeOutput = outputInfo;
            if (!string.IsNullOrEmpty(errorInfo))
            {
                var exception = new HqvException("Error message from FFprobe");
                exception.Data["error-stream"] = errorInfo;
                throw exception;
            }

            if (string.IsNullOrEmpty(outputInfo))
            {
                throw new HqvException("No output message from FFprobe");
            }

            return JObject.Parse(outputInfo);
        }       
    }
}