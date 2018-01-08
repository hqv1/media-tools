using System;
using System.IO;
using Hqv.CSharp.Common.Audit;
using Hqv.MediaTools.Console.Options;
using Hqv.MediaTools.Types.FileDownload;

namespace Hqv.MediaTools.Console.Actors
{
    internal class DownloadFileActor
    {
        private readonly IAuditorResponseBase _auditor;
        private readonly IFileDownloaderService _fileDownloaderService;
        private string _correlationId;

        public DownloadFileActor(
            IAuditorResponseBase auditor,
            IFileDownloaderService fileDownloaderService)
        {
            _auditor = auditor;
            _fileDownloaderService = fileDownloaderService;
        }

        public int Act(DownloadFileOptions options)
        {
            _correlationId = Guid.NewGuid().ToString();

            var outputFileName = options.FileName;
            if (string.IsNullOrEmpty(outputFileName))
            {
                outputFileName = Path.GetFileNameWithoutExtension(options.Url);
            }

            var extension = options.Extension;
            if (string.IsNullOrEmpty(extension))
            {
                extension = Path.GetExtension(options.Url);
            }

            var request = new DownloadRequest(
                url: options.Url,
                outputFileName: outputFileName,
                outputExtension: extension,
                correlationId: _correlationId);

            var response = _fileDownloaderService.Download(request).Result;
            if (response.IsValid)
            {
                _auditor.AuditSuccess("Url", request.Url, "FileDownloaded", response);
                return 0;
            }

            _auditor.AuditFailure("Url", request.Url, "FileDownloadFailed", response);
            return 1;
        }
    }
}