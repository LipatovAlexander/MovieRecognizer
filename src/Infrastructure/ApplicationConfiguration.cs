using Amazon.S3;
using Application;
using Application.Videos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VideoLibrary;

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

    public static void AddAmazonS3Client(this IServiceCollection services)
    {
        var serviceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL");

        if (serviceUrl is null)
        {
            services.AddAWSService<IAmazonS3>();
        }
        else
        {
            services.AddSingleton<IAmazonS3>(new AmazonS3Client(new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true
            }));
        }
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(Client.For(YouTube.Default));
        services.AddSingleton<IVideoService, VideoService>();
    }
}