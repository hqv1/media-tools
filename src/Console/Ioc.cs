using System;
using System.Linq;
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
using Hqv.Seedwork.Audit.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Hqv.MediaTools.Console
{
    internal static class Ioc
    {
        public static IServiceProvider RegisterComponents(IConfigurationRoot configuration)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddOptions();
            
            services.AddScoped<ILogger>(provider => new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());

            services.AddScoped<CreateThumbnailsActor>();
            services.AddScoped<CreateThumbnailSheetActor>();
            services.AddScoped<DownloadFileActor>();

            services.AddScoped<IAuditorResponseBase, AuditorResponseBase>();
            services.Configure<AuditorResponseBase.Config>(configuration.GetSection("Auditing"));

            services.Configure<FileDownloaderService.Config>(configuration.GetSection(FileDownloaderService.Config.ConfigurationSectionName));
            services.AddScoped<IFileDownloaderService, FileDownloaderService>();

            services.Configure<ThumbnailCreationNotAccurateService.Config>(configuration.GetSection(ThumbnailCreationNotAccurateService.Config.ConfigurationSectionName));
            services.AddScoped<IThumbnailCreationService, ThumbnailCreationNotAccurateService>();

            services.Configure<ThumbnailSheetCreationService.Config>(configuration.GetSection(ThumbnailSheetCreationService.Config.ConfigurationSectionName));
            services.AddScoped<IThumbnailSheetCreationService, ThumbnailSheetCreationService>();

            services.Configure<VideoFileInfoExtractionService.Config>(configuration.GetSection(VideoFileInfoExtractionService.Config.ConfigurationSectionName));
            services.AddScoped<IVideoFileInfoExtractionService, VideoFileInfoExtractionService>();

            return services.BuildServiceProvider();
        }
        
    }    
}