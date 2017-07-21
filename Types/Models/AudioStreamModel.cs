namespace Hqv.MediaTools.Types.Models
{
    /// <summary>
    /// Audio stream
    /// </summary>
    public struct AudioStreamModel
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
    }
}