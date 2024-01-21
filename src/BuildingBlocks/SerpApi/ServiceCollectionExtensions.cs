using Microsoft.Extensions.DependencyInjection;

namespace SerpApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerpApi(this IServiceCollection services, string apiKey)
    {
        var settings = new SerpApiSettings
        {
            ApiKey = apiKey
        };

        services.AddSingleton(settings);

        services.AddHttpClient(Constants.HttpClientName)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://serpapi.com/");
            });
        
        return services
            .AddSingleton<IYandexReverseImageApi, YandexReverseImageApi>();
    }
}