using Microsoft.Extensions.DependencyInjection;

namespace Application.SerpApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerpApi(this IServiceCollection services, string apiKey)
    {
        var settings = new SerpApiSettings
        {
            ApiKey = apiKey
        };

        services.AddSingleton(settings);

        services.AddHttpClient<IYandexReverseImageApi, YandexReverseImageApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://serpapi.com/");
            });
        
        return services
            .AddSingleton<IYandexReverseImageApi, YandexReverseImageApi>();
    }
}