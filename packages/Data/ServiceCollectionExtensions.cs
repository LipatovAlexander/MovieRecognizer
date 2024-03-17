using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ServiceCollectionExtensions
{
    public static void AddData(this IServiceCollection services)
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

        services.AddSingleton<IRepository<MovieRecognition, Guid>, MovieRecognitionRepository>();
        services.AddSingleton<IRepository<Video, Guid>, VideoRepository>();

        services.AddSingleton<IDatabaseContext, DatabaseContext>();
    }
}