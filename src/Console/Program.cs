using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using CommandLine;
using Hqv.MediaTools.Console.Actors;
using Hqv.MediaTools.Console.Options;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Hqv.MediaTools.Console
{
    /// <summary>
    /// This application can do the following.
    /// Create thumbnails every X seconds    
    /// Create a thumbnail sheet.
    /// Download a file using FFmpeg
    /// 
    /// Run the command line to see all the options available to it.
    /// Configuration are stored in appsettings.json
    /// Errors are logged into a log file.
    /// 
    /// </summary>
    internal class Program
    {
        private static IConfigurationRoot _config;
        private static IContainer _iocContainer;

        private static int Main(string[] args)
        {
            GetConfigurationRoot();
            _iocContainer = Ioc.RegisterComponents(_config);

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
                var logger = _iocContainer.Resolve<ILogger>();
                logger.Error(ex, "Fatal exception");
                System.Console.WriteLine($"Exception. See logs: {ex.Message}");
                return 1;
            }
        }

        private static int ProcessError(IEnumerable<Error> errs, string[] args)
        {
            var logger = _iocContainer.Resolve<ILogger>();            
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
    }
}