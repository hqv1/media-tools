using System.IO;
using System.Linq;
using Hqv.MediaTools.Domain;
using ImageMagick;

namespace Domain.ThumbnailSheet
{
    internal class SheetCreator
    {
        private readonly ThumbnailSheetService.Settings _settings;

        public SheetCreator(ThumbnailSheetService.Settings settings)
        {
            _settings = settings;
        }

        public void CreateSheet(ThumbnailSheetCreateRequest request, ThumbnailSheetService.Response response)
        {
            int tempWidth = 0, tempHeight = 0;
            using (var images = new MagickImageCollection())
            {
                var files = Directory.GetFiles(_settings.TempThumbnailPath).OrderBy(x => x).ToList();
                foreach (var file in files)
                {
                    var image = new MagickImage(file);
                    tempWidth = image.Width;
                    tempHeight = image.Height;
                    images.Add(image);
                }

                var conversionNumber = tempWidth / request.ThumbnailWidth;
                tempHeight = tempHeight / conversionNumber;

                var montageSetting = new MontageSettings
                {
                    Geometry = new MagickGeometry(request.ThumbnailWidth, tempHeight),
                    TileGeometry = new MagickGeometry(GetTileGemetry(files.Count)),
                    BackgroundColor = MagickColors.Black,
                    BorderColor = MagickColors.DarkGray,
                    BorderWidth = 1,
                    Font = "Arial",
                    FontPointsize = request.SheetTitleFontSize,
                    FillColor = MagickColors.LightGray,
                    StrokeColor = MagickColors.WhiteSmoke,
                    Title = request.SheetName
                };

                using (var montage = images.Montage(montageSetting))
                {
                    var path = Path.Combine(_settings.ThumbnailSheetPath, request.SheetName + "-sheet.jpg");
                    montage.Format = MagickFormat.Jpeg;
                    montage.Quality = request.SheetQuality;
                    montage.Write(path);
                    response.SheetFilePath = path;
                }
            }
        }

        private static string GetTileGemetry(int thumbnails)
        {
            var value = 10;
            if (thumbnails <= 3)
                value = 1;
            else if (thumbnails <= 6)
                value = 2;
            else if (thumbnails <= 9)
                value = 3;
            else if (thumbnails <= 16)
                value = 4;
            else if (thumbnails <= 25)
                value = 5;
            else if (thumbnails <= 36)
                value = 6;
            else if (thumbnails <= 49)
                value = 7;
            else if (thumbnails <= 64)
                value = 8;
            else if (thumbnails <= 81)
                value = 9;
            return $"{value}x";
        }
    }
}