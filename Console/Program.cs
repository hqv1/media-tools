using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using CommandLine;
using Hqv.CSharp.Common.Audit;
using Hqv.CSharp.Common.Audit.Logger;
using Hqv.CSharp.Common.Log;
using Hqv.CSharp.Common.Log.NLog;
using Hqv.MediaTools.Console.Actors;
using Hqv.MediaTools.ThumbnailSheet;
using Hqv.MediaTools.Types.ThumbnailSheet;
using Hqv.MediaTools.Types.VideoFileInfo;
using Hqv.MediaTools.VideoFileInfo;
using Microsoft.Extensions.Configuration;

namespace Hqv.MediaTools.Console
{
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
                return Parser.Default.ParseArguments<CreateThumbnailSheetOptions>(args)
                    .MapResult(
                        CreateThumbnailSheet,
                        errs=>ProcessError(errs, args)
                    );
            }
            catch (Exception ex)
            {
                var logger = _iocContainer.Resolve<ILogger>();
                logger.LogError("Fatal exception", ex);
                System.Console.WriteLine($"Exception. See logs: {ex.Message}");
                return 1;
            }
        }

        

        private static int CreateThumbnailSheet(CreateThumbnailSheetOptions opts)
        {
            var actor = _iocContainer.Resolve<CreateThumbnailSheetActor>();
            actor.Act(opts);
            return 0;
        }

        private static int ProcessError(IEnumerable<Error> errs, string[] args)
        {
            var logger = _iocContainer.Resolve<ILogger>();            
            var exception = new Exception("Unable to parse command");
            exception.Data["args"] = string.Join("; ", args) + " --- ";
            exception.Data["errors"] = string.Join("; ", errs.Select(x=>x.Tag));
            logger.LogError("Exiting programming", exception);

            return 0;
        }

        private static void GetConfigurationRoot()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        /// <summary>
        /// IOC container using Autofac (https://autofac.org/)
        /// </summary>
        private static void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_config).As<IConfiguration>();

            builder.RegisterType<Logger>().As<ILogger>();
            builder.Register(x => NLog.LogManager.GetLogger("console")).As<NLog.ILogger>();

            builder.RegisterType<CreateThumbnailSheetActor>();

            builder.RegisterType<BusinessAuditorResponseBase>().As<IAuditorResponseBase>();
            builder.RegisterInstance(new BusinessAuditorResponseBase.Settings(
                Convert.ToBoolean(_config["auditing:audit-on-successful-event"]),
                Convert.ToBoolean(_config["auditing:detail-audit-on-successful-event"])));
            
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