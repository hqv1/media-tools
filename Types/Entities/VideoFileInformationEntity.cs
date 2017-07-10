namespace Hqv.MediaTools.Types.Entities
{
    /// <summary>
    /// Video file information
    /// </summary>
    public class VideoFileInformationEntity
    {
        /// <summary>
        /// Video file path
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// File name (no extension)
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Extension (with the .)
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Video file format 
        /// </summary>
        public string FormatName { get; set; }
        /// <summary>
        /// Bit rate
        /// </summary>
        public string BitRate { get; set; }
        /// <summary>
        /// File size (in bytes)
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// Duration in seconds
        /// </summary>
        public double DurationInSecs { get; set; }

        /// <summary>
        /// Video stream information. If the file contains more than one video stream, use the first one.
        /// </summary>
        public VideoStreamEntity VideoStream { get; set; }
        /// <summary>
        /// Audio stream information. If the file contains more than one audio stream, use the first one
        /// </summary>
        public AudioStreamEntity AudioStream { get; set; }
    }
}