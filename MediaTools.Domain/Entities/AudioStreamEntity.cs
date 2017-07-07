namespace Hqv.MediaTools.Domain.Entities
{
    public class AudioStreamEntity
    {
        public string CodecName { get; set; }
        public double StartTime { get; set; }
        public int Channels { get; set; }
        public string ChannelLayout { get; set; }
    }
}