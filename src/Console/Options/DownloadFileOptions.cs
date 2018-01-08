using CommandLine;

namespace Hqv.MediaTools.Console.Options
{
    [Verb("download-file", HelpText = "Download file")]
    internal class DownloadFileOptions
    {
        [Option('u', "url", Required = true, HelpText = "Url to download")]
        public string Url { get; set; }

        [Option('f', "filename", Required = false, HelpText = "Filename to save as")]
        public string FileName { get; set; }

        [Option('e', "ext", Required = false, HelpText = "Extension to save as")]
        public string Extension { get; set; }
    }
}