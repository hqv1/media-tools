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
    }
}