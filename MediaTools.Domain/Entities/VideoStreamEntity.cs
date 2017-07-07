namespace Hqv.MediaTools.Domain.Entities
{
    public class VideoStreamEntity
    {        
        public int Width { get; set; }
        public int Height { get; set; }
        public string CodecName { get; set; }
        public double StartTime { get; set; }
    }
}