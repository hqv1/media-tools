using CommandLine;

namespace Hqv.MediaTools.Console
{
    /// <summary>
    /// Used by command line library (https://github.com/gsscoder/commandline).
    /// </summary>
    [Verb("create-ts", HelpText = "Create thumbnail sheet")]
    internal class CreateThumbnailSheetOptions 
    {
        [Option('v', "video-file-path", Required = true, HelpText = "Path to video file")]
        public string VideoFilePath { get; set; }

        [Option('t', "nbr-thumbnails", Required = true, HelpText = "Number of thumbnails")]
        public int NumberOfThumbnails { get; set; }
    }
}