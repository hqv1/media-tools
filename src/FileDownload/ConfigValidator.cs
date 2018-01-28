using System.IO;
using FluentValidation;

namespace Hqv.MediaTools.FileDownload
{
    public class ConfigValidator : AbstractValidator<FileDownloaderService.Config>
    {
        public ConfigValidator()
        {
            RuleFor(x => x.SavePath).Must(Directory.Exists);
            RuleFor(x => x.FfmpegPath).Must(File.Exists);
        }
    }
}