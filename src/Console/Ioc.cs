using System;
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
            services.Configure<AuditorResponseBase.Config>(config=>
                configuration.GetSection("Auditing"));

            services.AddScoped<IFileDownloaderService, FileDownloaderService>();
            services.AddScoped(provider => new FileDownloaderService.Settings(
                configuration["ffmpeg-path"],
                configuration["file-downloader:save-path"]));

            services.AddScoped<IThumbnailCreationService, ThumbnailCreationNotAccurateService>();
            services.AddScoped(provider => new ThumbnailCreationNotAccurateService.Settings(
                configuration["thumbnail:thumbnail-path"],
                configuration["ffmpeg-path"]));


            services.AddScoped<IThumbnailSheetCreationService, ThumbnailSheetCreationService>();
            services.AddScoped(provider => new ThumbnailSheetCreationService.Settings(
                configuration["thumbnailsheet:thumbnail-path-temp"],
                configuration["thumbnailsheet:sheet-path"],
                configuration["ffmpeg-path"]));            

            services.AddScoped<IVideoFileInfoExtractionService, VideoFileInfoExtractionService>();
            services.AddScoped(provider => Microsoft.Extensions.Options.Options.Create(new VideoFileInfoExtractionService.Config(
                configuration["ffprobe-path"])));

            return services.BuildServiceProvider();
        }
    }    
}