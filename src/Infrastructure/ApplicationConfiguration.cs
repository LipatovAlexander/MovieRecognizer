using Amazon.S3;
using Application;
using Application.Files;
using Application.Videos;
using Infrastructure.Files;
using Infrastructure.Videos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YoutubeExplode;

namespace Infrastructure;

public static class ApplicationConfiguration
{
    public static void AddApplicationDbContext(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("application",
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    dbContextOptionsBuilder.EnableSensitiveDataLogging();
                }
            });
    
        builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }

    public static void AddAmazonS3Client(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceUrl = configuration.GetRequiredSection(FileStorageSettings.SectionName)["ServiceUrl"];
        
        if (serviceUrl is null)
        {
            services.AddAWSService<IAmazonS3>();
        }
        else
        {
            services.AddSingleton<IAmazonS3>(new AmazonS3Client(new AmazonS3Config
            {
                ServiceURL = serviceUrl
            }));
        }
    }

    public static void AddApplicationServices(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddSingleton(new YoutubeClient());
        services.AddScoped<IVideoService, VideoService>();
        services.AddSingleton<IVideoConverter, VideoConverter>();

        services.AddOptions<FileStorageSettings>()
            .BindConfiguration(FileStorageSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<IFileStorage, FileStorage>();
    }
}