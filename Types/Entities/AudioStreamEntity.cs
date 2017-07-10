namespace Hqv.MediaTools.Types.Entities
{
    /// <summary>
    /// Audio stream
    /// </summary>
    public class AudioStreamEntity
    {
        /// <summary>
        /// Codec name
        /// </summary>
        public string CodecName { get; set; }
        /// <summary>
        /// Start time
        /// </summary>
        public double StartTime { get; set; }
        /// <summary>
        /// Number of channels
        /// </summary>
        public int Channels { get; set; }
        /// <summary>
        /// Channel layout
        /// </summary>
        public string ChannelLayout { get; set; }
    }
}