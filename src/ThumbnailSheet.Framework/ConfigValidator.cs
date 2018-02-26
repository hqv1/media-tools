using System.IO;
using FluentValidation;

namespace Hqv.MediaTools.ThumbnailSheet.Framework
{
    /// <inheritdoc />
    /// <summary>
    /// Validate Config
    /// </summary>
    internal class ConfigValidator : AbstractValidator<ThumbnailSheetCreationService.Config>
    {
        public ConfigValidator()
        {
            RuleFor(x => x.TempThumbnailPath).Must(Directory.Exists);
            RuleFor(x => x.ThumbnailSheetPath).Must(Directory.Exists);
            RuleFor(x => x.FfmpegPath).Must(File.Exists);
        }
    }
}
