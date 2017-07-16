using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using CommandLine;
using Hqv.CSharp.Common.Audit;
using Hqv.CSharp.Common.Audit.Logging;
using Hqv.CSharp.Common.Logging;
using Hqv.CSharp.Common.Logging.NLog;
using Hqv.MediaTools.Console.Actors;
using Hqv.MediaTools.Console.Options;
using Hqv.MediaTools.FileDownload;
using Hqv.MediaTools.Thumbnail;
using Hqv.MediaTools.ThumbnailSheet;
using Hqv.MediaTools.Types.FileDownload;
using Hqv.MediaTools.Types.Thumbnail;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.MediaTools.Types.VideoFileInfo;
using Hqv.MediaTools.VideoFileInfo;
using Microsoft.Extensions.Configuration;

namespace Hqv.MediaTools.Console
{
    /// <summary>
    /// Creates a thumbnail sheet.
    /// 
    /// Use IVideoFileInfoExtractionService to extract the video file information. The only data we're looking for is 
    /// the duration. Then use IThumbnailSheetCreationService to create the thumbnail sheet.
    /// 
    /// This console apps logs to a file on success and errors. You can configure the logs in appsettings.json and in 
    /// nlog.config.
    /// </summary>
    internal class Program
    {
        private static IConfigurationRoot _config;
        private static IContainer _iocContainer;

        private static int Main(string[] args)
        {
            GetConfigurationRoot();
            RegisterComponents();

            try
            {
                return Parser.Default.ParseArguments<
                    CreateThumbnailsOptions, 
                    CreateThumbnailSheetOptions,
                    DownloadFileOptions>(args)
                    .MapResult(
                        (CreateThumbnailsOptions opts) => _iocContainer.Resolve<CreateThumbnailsActor>().Act(opts),
                        (CreateThumbnailSheetOptions opts) => _iocContainer.Resolve<CreateThumbnailSheetActor>().Act(opts),
                        (DownloadFileOptions opts) => _iocContainer.Resolve<DownloadFileActor>().Act(opts),
                        errs=>ProcessError(errs, args)
                    );
            }
            catch (Exception ex)
            {
                var logger = _iocContainer.Resolve<IHqvLogger>();
                logger.Error(ex, "Fatal exception");
                System.Console.WriteLine($"Exception. See logs: {ex.Message}");
                return 1;
            }
        }

        private static int ProcessError(IEnumerable<Error> errs, string[] args)
        {
            var logger = _iocContainer.Resolve<IHqvLogger>();            
            var exception = new Exception("Unable to parse command");
            exception.Data["args"] = string.Join("; ", args) + " --- ";
            exception.Data["errors"] = string.Join("; ", errs.Select(x=>x.Tag));
            logger.Error(exception, "Exiting programming");

            return 1;
        }

        private static void GetConfigurationRoot()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)                
                .Build();
        }

        /// <summary>
        /// IOC container using Autofac (https://autofac.org/)
        /// </summary>
        private static void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_config).As<IConfiguration>();

            builder.RegisterType<Logger>().As<IHqvLogger>();
            builder.Register(x => NLog.LogManager.GetLogger("console")).As<NLog.ILogger>();

            builder.RegisterType<CreateThumbnailsActor>();
            builder.RegisterType<CreateThumbnailSheetActor>();
            builder.RegisterType<DownloadFileActor>();

            builder.RegisterType<AuditorResponseBase>().As<IAuditorResponseBase>();
            builder.RegisterInstance(new AuditorResponseBase.Settings(
                Convert.ToBoolean(_config["auditing:audit-on-successful-event"]),
                Convert.ToBoolean(_config["auditing:detail-audit-on-successful-event"])));

            builder.RegisterType<FileDownloaderService>().As<IFileDownloaderService>();
            builder.RegisterInstance(new FileDownloaderService.Settings(
                _config["ffmpeg-path"],
                _config["file-downloader:save-path"]));

            builder.RegisterType<ThumbnailCreationNotAccurateService>().As<IThumbnailCreationService>();
            builder.RegisterInstance(new ThumbnailCreationNotAccurateService.Settings(
                _config["thumbnail:thumbnail-path"],
                _config["ffmpeg-path"]));

            builder.RegisterType<ThumbnailSheetCreationService>().As<IThumbnailSheetCreationService>();
            builder.RegisterInstance(new ThumbnailSheetCreationService.Settings(
                _config["thumbnailsheet:thumbnail-path-temp"],
                _config["thumbnailsheet:sheet-path"],
                _config["ffmpeg-path"]));

            builder.RegisterType<VideoFileInfoExtractionService>().As<IVideoFileInfoExtractionService>();
            builder.RegisterInstance(new VideoFileInfoExtractionService.Settings(
                _config["ffprobe-path"]));
            
            _iocContainer = builder.Build();
        }        
    }
}