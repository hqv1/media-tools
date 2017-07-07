namespace Hqv.MediaTools.Domain.Entities
{
    /// <summary>
    /// todo: add the rest of the data gathered from 
    /// </summary>
    public class VideoFileInformationEntity
    {
        public string FullPath { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }

        public string FormatName { get; set; }
        public string BitRate { get; set; }
        public long FileSize { get; set; }
        public double DurationInSecs { get; set; }

        public VideoStreamEntity VideoStream { get; set; }
        public AudioStreamEntity AudioStream { get; set; }
    }
}