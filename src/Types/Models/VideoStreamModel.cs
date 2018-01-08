namespace Hqv.MediaTools.Types.Models
{
    /// <summary>
    /// Video stream 
    /// </summary>
    public struct VideoStreamModel
    {
        public VideoStreamModel(int width, int height, string codecName, double startTime)
        {
            Width = width;
            Height = height;
            CodecName = codecName;
            StartTime = startTime;
        }
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Codec name
        /// </summary>
        public string CodecName { get; }
        /// <summary>
        /// Start time
        /// </summary>
        public double StartTime { get; }

        public bool Equals(VideoStreamModel other)
        {
            return Width == other.Width && Height == other.Height && string.Equals(CodecName, other.CodecName) &&
                   StartTime.Equals(other.StartTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VideoStreamModel && Equals((VideoStreamModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Width;
                hashCode = (hashCode * 397) ^ Height;
                hashCode = (hashCode * 397) ^ (CodecName != null ? CodecName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ StartTime.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VideoStreamModel left, VideoStreamModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VideoStreamModel left, VideoStreamModel right)
        {
            return !left.Equals(right);
        }
    }
}