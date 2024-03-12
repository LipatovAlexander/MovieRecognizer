using Data.Repositories;
using Data.YandexDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ServiceCollectionExtensions
{
    public static void AddData(this IServiceCollection services)
    {
        services.AddYandexDbService();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRecognitionRepository, MovieRecognitionRepository>();
        services.AddSingleton<IVideoRepository, VideoRepository>();
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
    }
}