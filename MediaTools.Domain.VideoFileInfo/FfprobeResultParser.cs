using System.IO;
using System.Linq;
using Hqv.CSharp.Common.Exceptions;
using Hqv.MediaTools.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace MediaTools.Domain.VideoFileInfo
{
    internal class FfprobeResultParser
    {
        public VideoFileInformationEntity Parse(JObject json)
        {
            dynamic videoStream = json["streams"].FirstOrDefault(x => Extensions.Value<string>(x["codec_type"]) == "video");
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

            var path = format.file_name;
            var filename = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            var formatName = format.format_name;
            var bitRate = format.bit_rate;
            var filesize = format.size;
            double duration = format.duration;


            var videoFile = new VideoFileInformationEntity
            {
                FullPath = path,
                Filename = filename,
                Extension = extension,
                FormatName = formatName,
                BitRate = bitRate,
                FileSize = filesize,
                DurationInSecs = duration,

                VideoStream = GetVideoStream(videoStream),
                AudioStream = GetAudioStream(audioStream)
            };
            return videoFile;
        }

        private static VideoStreamEntity GetVideoStream(dynamic videoStream)
        {
            var width = videoStream.width;
            var height = videoStream.height;
            double startTime = videoStream.start_time;
            var codecName = videoStream.codec_name;

            return new VideoStreamEntity
            {
                Width = width,
                Height = height,
                StartTime = startTime,
                CodecName = codecName,
            };
        }

        private static AudioStreamEntity GetAudioStream(dynamic audioStream)
        {
            var codecName = audioStream.codec_name;
            double startTime = audioStream.start_time;
            int channels = audioStream.channels;
            var channelLayout = audioStream.channel_layout;

            return new AudioStreamEntity
            {
                CodecName = codecName,
                StartTime = startTime,
                Channels = channels,
                ChannelLayout = channelLayout
            };
        }
    }
}