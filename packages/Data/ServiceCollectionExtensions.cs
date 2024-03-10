using Data.Repositories;
using Data.YandexDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace Data;

public static class ServiceCollectionExtensions
{
    public static void AddData(this IServiceCollection services)
    {
        services.AddYandexDbService();
        services.AddPollyPipelines();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRecognitionRepository, MovieRecognitionRepository>();
        services.Decorate<IMovieRecognitionRepository, ResilientMovieRecognitionRepository>();
    }

    private static void AddPollyPipelines(this IServiceCollection services)
    {
        services.AddResiliencePipeline("repository-get", builder =>
        {
            builder.AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    DelayGenerator = static args =>
                    {
                        var delay = args.AttemptNumber switch
                        {
                            0 => TimeSpan.Zero,
                            1 => TimeSpan.FromMilliseconds(10),
                            2 => TimeSpan.FromMilliseconds(20),
                            3 => TimeSpan.FromMilliseconds(30),
                            _ => TimeSpan.FromMilliseconds(100)
                        };

                        return new ValueTask<TimeSpan?>(delay);
                    }
                })
                .AddTimeout(TimeSpan.FromSeconds(3));
        });

        services.AddResiliencePipeline("repository-save", builder =>
        {
            builder.AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    DelayGenerator = static args =>
                    {
                        var delay = args.AttemptNumber switch
                        {
                            0 => TimeSpan.Zero,
                            1 => TimeSpan.FromMilliseconds(10),
                            2 => TimeSpan.FromMilliseconds(20),
                            3 => TimeSpan.FromMilliseconds(30),
                            _ => TimeSpan.FromMilliseconds(100)
                        };

                        return new ValueTask<TimeSpan?>(delay);
                    }
                })
                .AddTimeout(TimeSpan.FromSeconds(3));
        });
    }

    private static void AddYandexDbService(this IServiceCollection services)
    {
        services.AddOptions<YandexDbSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                settings.Endpoint = configuration["YDB_ENDPOINT"]
                                    ?? throw new InvalidOperationException(
                                        "Required configuration YDB_ENDPOINT not found");

                settings.Database = configuration["YDB_DATABASE_PATH"]
                                    ?? throw new InvalidOperationException(
                                        "Required configuration YDB_DATABASE_PATH not found");
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IYandexDbService, YandexDbService>();

        services.AddHostedService<YandexDbServiceInitializer>();
    }
}