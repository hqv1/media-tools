namespace Hqv.MediaTools.Types.Models
{
    /// <summary>
    /// Video file information
    /// </summary>
    public struct VideoFileInformationModel
    {
        public VideoFileInformationModel(string fullPath, string filename, string extension, string formatName,
            string bitRate, long fileSize, double durationInSecs, VideoStreamModel videoStream,
            AudioStreamModel audioStream)
        {
            FullPath = fullPath;
            Filename = filename;
            Extension = extension;
            FormatName = formatName;
            BitRate = bitRate;
            FileSize = fileSize;
            DurationInSecs = durationInSecs;
            VideoStream = videoStream;
            AudioStream = audioStream;
        }

        /// <summary>
        /// Video file path
        /// </summary>
        public string FullPath { get; }
        /// <summary>
        /// File name (no extension)
        /// </summary>
        public string Filename { get; }
        /// <summary>
        /// Extension (with the .)
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// Video file format 
        /// </summary>
        public string FormatName { get; }
        /// <summary>
        /// Bit rate
        /// </summary>
        public string BitRate { get; }
        /// <summary>
        /// File size (in bytes)
        /// </summary>
        public long FileSize { get; }
        /// <summary>
        /// Duration in seconds
        /// </summary>
        public double DurationInSecs { get; }

        /// <summary>
        /// Video stream information. If the file contains more than one video stream, use the first one.
        /// </summary>
        public VideoStreamModel VideoStream { get; set; }
        /// <summary>
        /// Audio stream information. If the file contains more than one audio stream, use the first one
        /// </summary>
        public AudioStreamModel AudioStream { get; set; }
    }
}