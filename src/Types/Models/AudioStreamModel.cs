namespace Hqv.MediaTools.Types.Models
{
    /// <summary>
    /// Audio stream
    /// </summary>
    public class AudioStreamModel
    {
        public AudioStreamModel(string codecName, double startTime, int channels, string channelLayout)
        {
            CodecName = codecName;
            StartTime = startTime;
            Channels = channels;
            ChannelLayout = channelLayout;
        }

        /// <summary>
        /// Codec name
        /// </summary>
        public string CodecName { get;}
        /// <summary>
        /// Start time
        /// </summary>
        public double StartTime { get; }
        /// <summary>
        /// Number of channels
        /// </summary>
        public int Channels { get; }
        /// <summary>
        /// Channel layout
        /// </summary>
        public string ChannelLayout { get; }

        public bool Equals(AudioStreamModel other)
        {
            return string.Equals(CodecName, other.CodecName) && StartTime.Equals(other.StartTime) &&
                   Channels == other.Channels && string.Equals(ChannelLayout, other.ChannelLayout);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AudioStreamModel && Equals((AudioStreamModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (CodecName != null ? CodecName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ StartTime.GetHashCode();
                hashCode = (hashCode * 397) ^ Channels;
                hashCode = (hashCode * 397) ^ (ChannelLayout != null ? ChannelLayout.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(AudioStreamModel left, AudioStreamModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AudioStreamModel left, AudioStreamModel right)
        {
            return !left.Equals(right);
        }
    }
}