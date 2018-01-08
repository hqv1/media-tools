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
        public VideoStreamModel VideoStream { get; }
        /// <summary>
        /// Audio stream information. If the file contains more than one audio stream, use the first one
        /// </summary>
        public AudioStreamModel AudioStream { get; }

        public bool Equals(VideoFileInformationModel other)
        {
            return string.Equals(FullPath, other.FullPath) && string.Equals(Filename, other.Filename) &&
                   string.Equals(Extension, other.Extension) && string.Equals(FormatName, other.FormatName) &&
                   string.Equals(BitRate, other.BitRate) && FileSize == other.FileSize &&
                   DurationInSecs.Equals(other.DurationInSecs) && VideoStream.Equals(other.VideoStream) &&
                   AudioStream.Equals(other.AudioStream);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VideoFileInformationModel && Equals((VideoFileInformationModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (FullPath != null ? FullPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Filename != null ? Filename.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Extension != null ? Extension.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FormatName != null ? FormatName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (BitRate != null ? BitRate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ FileSize.GetHashCode();
                hashCode = (hashCode * 397) ^ DurationInSecs.GetHashCode();
                hashCode = (hashCode * 397) ^ VideoStream.GetHashCode();
                hashCode = (hashCode * 397) ^ AudioStream.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VideoFileInformationModel left, VideoFileInformationModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VideoFileInformationModel left, VideoFileInformationModel right)
        {
            return !left.Equals(right);
        }
    }
}