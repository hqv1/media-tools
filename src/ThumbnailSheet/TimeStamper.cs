using System;
using System.IO;
using Hqv.MediaTools.Types.ThumbnailSheet;
using ImageMagick;

namespace Hqv.MediaTools.ThumbnailSheet
{
    /// <summary>
    /// Add a time stamp to an image
    /// </summary>
    internal class TimeStamper
    {               
        public void Stamp(ThumbnailSheetCreateRequest request, string filePath, TimeSpan time)
        {
            var stampText = time.ToString(request.VideoDurationInSeconds >= 3600 ? @"hh\:mm\:ss" : @"mm\:ss");
            var tempFilePath = filePath + ".tmp.png";

            using (var imgText = new MagickImage(filePath))            
            {
                var drawable = new DrawableText(5, 5, stampText);
                var gravity = new DrawableGravity(Gravity.Southeast);
                var font = new DrawableFont("Tahoma");
                var antialias = new DrawableTextAntialias(true);
                var size = new DrawableFontPointSize(48);
                var color = new DrawableFillColor(MagickColors.Black);
                var strokecolor = new DrawableStrokeColor(MagickColors.AliceBlue);
                imgText.Draw(drawable, gravity, font, antialias, size, color, strokecolor);
                imgText.Write(tempFilePath);
            }

            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }
    }
}