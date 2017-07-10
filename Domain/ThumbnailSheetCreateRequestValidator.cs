using System.IO;
using FluentValidation;

namespace Hqv.MediaTools.Domain
{
    /// <summary>
    /// Validate the ThumbnailSheetCreateRequest
    /// </summary>
    public class ThumbnailSheetCreateRequestValidator : AbstractValidator<ThumbnailSheetCreateRequest>
    {
        public ThumbnailSheetCreateRequestValidator()
        {
            RuleFor(x => x.VideoPath).Must(File.Exists).WithMessage("Video does not exist");
            RuleFor(x => x.SheetName).NotEmpty();
            RuleFor(x => x.SheetQuality).GreaterThan(0).LessThanOrEqualTo(100);
            RuleFor(x => x.SheetTitleFontSize).GreaterThan(0);
            RuleFor(x => x.NumberOfThumbnails).GreaterThan(0);
            RuleFor(x => x.VideoDurationInSeconds).GreaterThan(0);
        }
    }
}