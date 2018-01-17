using System;
using Autofac;

using Hqv.MediaTools.Console.Actors;
using Hqv.MediaTools.FileDownload;
using Hqv.MediaTools.Thumbnail;
using Hqv.MediaTools.ThumbnailSheet;
using Hqv.MediaTools.Types.FileDownload;
using Hqv.MediaTools.Types.Thumbnail;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.MediaTools.Types.VideoFileInfo;
using Hqv.MediaTools.VideoFileInfo;
using Hqv.Seedwork.Audit;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Hqv.MediaTools.Console
{
    /// <summary>
    /// IOC container using Autofac (https://autofac.org/)
    /// </summary>
    internal static class Ioc
    {
        public static IContainer RegisterComponents(IConfigurationRoot config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(config).As<IConfiguration>();

            try
            {
                var loggingPath = config["logging:path"];
                if (string.IsNullOrEmpty(loggingPath))
                {
                    const string message = "logging:path cannot be empty in configuration file";
                    System.Console.WriteLine(message);
                    throw new Exception();
                }

                if (!Enum.TryParse(config["logging:minimum-level"], out LogEventLevel level))
                {
                    const string message = "logging:minimum-level is incorrect in configuration file";
                    System.Console.WriteLine(message);
                    throw new Exception();
                }
                var logLevelSwitch = new LoggingLevelSwitch { MinimumLevel = level };

                //builder.RegisterType<CSharp.Common.Logging.Serilog.Logger>().As<IHqvLogger>();
                builder.RegisterInstance(new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(logLevelSwitch)
                    .WriteTo.File(new JsonFormatter(), loggingPath)
                    .CreateLogger()).As<ILogger>();
            }
            catch (Exception)
            {
                System.Console.WriteLine("Logging registration failed");
                throw;
            }

            builder.RegisterType<CreateThumbnailsActor>();
            builder.RegisterType<CreateThumbnailSheetActor>();
            builder.RegisterType<DownloadFileActor>();

            //todo: remove or think about using IAuditorResponseBase
            //builder.RegisterType<AuditorResponseBase>().As<IAuditorResponseBase>();
            //builder.RegisterInstance(new AuditorResponseBase.Settings(
            //    Convert.ToBoolean(config["auditing:audit-on-successful-event"]),
            //    Convert.ToBoolean(config["auditing:detail-audit-on-successful-event"])));

            builder.RegisterType<FileDownloaderService>().As<IFileDownloaderService>();
            builder.RegisterInstance(new FileDownloaderService.Settings(
                config["ffmpeg-path"],
                config["file-downloader:save-path"]));

            builder.RegisterType<ThumbnailCreationNotAccurateService>().As<IThumbnailCreationService>();
            builder.RegisterInstance(new ThumbnailCreationNotAccurateService.Settings(
                config["thumbnail:thumbnail-path"],
                config["ffmpeg-path"]));

            builder.RegisterType<ThumbnailSheetCreationService>().As<IThumbnailSheetCreationService>();
            builder.RegisterInstance(new ThumbnailSheetCreationService.Settings(
                config["thumbnailsheet:thumbnail-path-temp"],
                config["thumbnailsheet:sheet-path"],
                config["ffmpeg-path"]));

            builder.RegisterType<VideoFileInfoExtractionService>().As<IVideoFileInfoExtractionService>();
            builder.RegisterInstance(new VideoFileInfoExtractionService.Settings(
                config["ffprobe-path"]));

            return builder.Build();
        }
    }
}