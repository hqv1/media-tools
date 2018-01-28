using System.IO;
using FluentValidation;

namespace Hqv.MediaTools.Thumbnail
{
    internal class ConfigValidator : AbstractValidator<ThumbnailCreationNotAccurateService.Config>
    {
        public ConfigValidator()
        {
            RuleFor(x => x.ThumbnailPath).Must(Directory.Exists);
            RuleFor(x => x.FfmpegPath).Must(File.Exists);
        }
    }
}