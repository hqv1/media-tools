using System.IO;
using System.Linq;
using Hqv.MediaTools.Types.ThumbnailSheet;
using ImageMagick;

namespace Hqv.MediaTools.ThumbnailSheet
{
    /// <summary>
    /// Create the thumbnail sheet. 
    /// 
    /// Uses MagickImage. We're using the Q8 version as we don't need the detail from the Q16 version. 
    /// 
    /// Assumptions: 
    ///   The thumbnail images are all the same dimensions. 
    ///   Using all the files in TempThumbnailPath to create the sheet.
    ///         
    /// </summary>
    internal class SheetCreator
    {
        private readonly ThumbnailSheetCreationService.Config _config;

        public SheetCreator(ThumbnailSheetCreationService.Config config)
        {
            _config = config;
        }

        public void CreateSheet(ThumbnailSheetCreateRequest request, ThumbnailSheetCreationService.Response response)
        {            
            using (var images = new MagickImageCollection())
            {
                CreateSheet(request, response, images);
            }
        }

        private void CreateSheet(ThumbnailSheetCreateRequest request, ThumbnailSheetCreationService.Response response, IMagickImageCollection images)
        {
            int tempWidth = 0, tempHeight = 0;
            var files = Directory.GetFiles(_config.TempThumbnailPath, "thumbnail*.png").OrderBy(x => x).ToList();
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
                TileGeometry = new MagickGeometry(GetTileGeometry(files.Count)),
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
                var path = Path.Combine(_config.ThumbnailSheetPath, request.SheetName + "-sheet.jpg");
                montage.Format = MagickFormat.Jpeg;
                montage.Quality = request.SheetQuality;
                montage.Write(path);
                response.SheetFilePath = path;
            }
        }

        /// <summary>
        /// Trying to create a square. Only goes to 10 columns.        
        /// </summary>
        private static string GetTileGeometry(int thumbnails)
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