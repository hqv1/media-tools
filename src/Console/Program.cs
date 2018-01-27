using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Hqv.MediaTools.Console.Actors;
using Hqv.MediaTools.Console.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private static IServiceProvider _iocContainer;

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
                        (CreateThumbnailsOptions opts) => _iocContainer.GetService<CreateThumbnailsActor>().Act(opts),
                        (CreateThumbnailSheetOptions opts) => _iocContainer.GetService<CreateThumbnailSheetActor>().Act(opts),
                        (DownloadFileOptions opts) => _iocContainer.GetService<DownloadFileActor>().Act(opts),
                        errs=>ProcessError(errs, args)
                    );
            }
            catch (Exception ex)
            {
                var logger = _iocContainer.GetService<ILogger>();
                logger.Error(ex, "Fatal exception");
                System.Console.WriteLine($"Exception. See logs: {ex.Message}");
                return 1;
            }
        }

        private static int ProcessError(IEnumerable<Error> errs, string[] args)
        {
            var logger = _iocContainer.GetService<ILogger>();            
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