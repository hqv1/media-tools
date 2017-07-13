using CommandLine;

namespace Hqv.MediaTools.Console
{
    /// <summary>
    /// Options for creating thumbnails
    /// </summary>
    [Verb("create-thumbnail", HelpText = "Create thumbnail")]
    internal class CreateThumbnailsOptions
    {
        [Option('v', "video-file-path", Required = true, HelpText = "Path to video file")]
        public string VideoFilePath { get; set; }

        [Option('t', "thumbnail-sec", Required = true, HelpText = "Get a thumbnail every x second")]
        public int ThumbnailsEverySecond { get; set; }
    }
}