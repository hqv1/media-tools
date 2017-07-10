namespace Hqv.MediaTools.Types.Entities
{
    /// <summary>
    /// Video stream 
    /// </summary>
    public class VideoStreamEntity
    {        
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Codec name
        /// </summary>
        public string CodecName { get; set; }
        /// <summary>
        /// Start time
        /// </summary>
        public double StartTime { get; set; }
    }
}