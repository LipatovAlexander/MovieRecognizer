using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Files;

public static class ServiceCollectionExtensions
{
    public static void AddFileStorage(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            var serviceUrl = configuration["AWS_S3_SERVICE_URL"]
                             ?? throw new InvalidOperationException(
                                 "Required configuration AWS_S3_SERVICE_URL not found");

            var regionEndpoint = configuration["AWS_DEFAULT_REGION"]
                                 ?? throw new InvalidOperationException(
                                     "Required configuration AWS_DEFAULT_REGION not found");

            return new AmazonS3Client(new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                AuthenticationRegion = regionEndpoint
            });
        });

        services.AddOptions<FileStorageSettings>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                var publicUrl = configuration["AWS_S3_SERVICE_URL"]
                                ?? throw new InvalidOperationException(
                                    "Required configuration AWS_S3_SERVICE_URL not found");

                var bucketName = configuration["S3_BUCKET"]
                                 ?? throw new InvalidOperationException(
                                     "Required configuration S3_BUCKET not found");

                options.PublicUrl = new Uri(publicUrl);
                options.BucketName = bucketName;
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IFileStorage, FileStorage>();
    }
}