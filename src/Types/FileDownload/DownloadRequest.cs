using System;
using System.Threading;
using FluentValidation;
using Hqv.Seedwork.Components;

namespace Hqv.MediaTools.Types.FileDownload
{
    public class DownloadRequest : RequestBase
    {
        public DownloadRequest(
            string url, 
            string outputFileName, 
            string outputExtension, 
            IProgress<string> progress = null, 
            CancellationToken cancellationToken = default(CancellationToken), 
            string correlationId = null)
            : base(correlationId ?? Guid.NewGuid().ToString())
        {
            Url = url;
            OutputFileName = outputFileName;
            OutputExtension = outputExtension;
            Progress = progress;
            CancellationToken = cancellationToken;
        }

        public string Url { get; }
        public string OutputFileName { get; }
        public string OutputExtension { get; }
        public IProgress<string> Progress { get;}
        public CancellationToken CancellationToken { get; }        
    }

    public class DownloadRequestValidator : AbstractValidator<DownloadRequest>
    {
        public DownloadRequestValidator()
        {
            RuleFor(x => x.Url).NotEmpty();
            RuleFor(x => x.OutputFileName).NotEmpty();
            RuleFor(x => x.OutputExtension).NotEmpty();
        }
    }
}