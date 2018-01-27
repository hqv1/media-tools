using System.IO;
using System.Linq;
using Hqv.MediaTools.Types.Models;
using Hqv.Seedwork.Exceptions;
using Newtonsoft.Json.Linq;

namespace Hqv.MediaTools.VideoFileInfo
{
    internal class FfprobeResultParser
    {
        public VideoFileInformationModel Parse(JObject json)
        {
            dynamic videoStream = json["streams"].FirstOrDefault(x => x["codec_type"].Value<string>() == "video");
            if (videoStream == null)
            {
                throw new HqvException("No video stream found");
            }

            dynamic format = json["format"];
            if (format == null)
            {
                throw new HqvException("No format found");
            }

            dynamic audioStream = json["streams"].FirstOrDefault(x => x["codec_type"].Value<string>() == "audio");

            string path = format.filename;
            var filename = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            string formatName = format.format_name;
            string bitRate = format.bit_rate;
            long filesize = format.size;
            double duration = format.duration;


            var videoFile = new VideoFileInformationModel(path, filename, extension, formatName, bitRate, filesize,
                duration, GetVideoStream(videoStream), GetAudioStream(audioStream));            
            return videoFile;
        }

        private static VideoStreamModel GetVideoStream(dynamic videoStream)
        {
            int width = videoStream.width;
            int height = videoStream.height;            
            string codecName = videoStream.codec_name;
            double startTime = videoStream.start_time;

            return new VideoStreamModel(width, height, codecName, startTime);
        }

        private static AudioStreamModel GetAudioStream(dynamic audioStream)
        {
            string codecName = audioStream.codec_name;
            double startTime = audioStream.start_time;
            int channels = audioStream.channels;
            string channelLayout = audioStream.channel_layout;

            return new AudioStreamModel(codecName, startTime, channels, channelLayout);
        }
    }
}