namespace Hqv.MediaTools.Domain
{
    public class ThumbnailSheetCreateRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="videoPath">Video path</param>
        /// <param name="sheetName">Sheet name</param>
        /// <param name="numberOfThumbnails">Number of thumbnails</param>
        /// <param name="videoDurationInSeconds">Duration of the video in seconds</param>
        /// <param name="shouldAddTimestamps">Should timestamps be added to each thumbnail?</param>
        /// <param name="sheetQuality">Quality of the outputed JPG</param>
        /// <param name="sheetTitleFontSize">Font size of the thumbnail sheet title.</param>
        /// <param name="thumbnailWidth">The width of each thumbnail.</param>
        public ThumbnailSheetCreateRequest(
            string videoPath,
            string sheetName,
            int numberOfThumbnails,
            double videoDurationInSeconds,
            bool shouldAddTimestamps = true,
            int sheetQuality = 80,
            int sheetTitleFontSize = 12,
            int thumbnailWidth = 320)
        {
            VideoPath = videoPath;
            SheetName = sheetName;
            SheetQuality = sheetQuality;
            SheetTitleFontSize = sheetTitleFontSize;
            NumberOfThumbnails = numberOfThumbnails;
            ThumbnailWidth = thumbnailWidth;
            VideoDurationInSeconds = videoDurationInSeconds;
            ShouldAddTimestamps = shouldAddTimestamps;
        }

        /// <summary>
        /// Full video path
        /// </summary>
        public string VideoPath { get; }

        /// <summary>
        /// Thumbnail sheet name. It'll be created by the service
        /// </summary>
        public string SheetName { get; }

        /// <summary>
        /// Number of thumbnails in the sheet.
        /// </summary>
        public int NumberOfThumbnails { get; }

        /// <summary>
        /// Video duration (in seconds)
        /// </summary>
        public double VideoDurationInSeconds { get; }

        /// <summary>
        /// Should timestamps be added to each thumbnail?
        /// </summary>
        public bool ShouldAddTimestamps { get; }

        /// <summary>
        /// The Thumbnail sheet will be in JPG format. Specify the quality of the JPG.
        /// </summary>
        public int SheetQuality { get; }

        /// <summary>
        /// Font size of the thumbnail sheet title. A large font size may cause the title to be clipped.
        /// </summary>
        public int SheetTitleFontSize { get; }
        
        /// <summary>
        /// The width of each thumbnail. The height will automatically be adjusted to keep the aspect ratio.
        /// </summary>
        public int ThumbnailWidth { get; }        
    }
}