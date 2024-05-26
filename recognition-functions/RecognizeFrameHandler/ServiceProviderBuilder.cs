using CloudFunctions;
using Data.Extensions;
using Files;
using MessageQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RecognizeFrameHandler;

public class ServiceProviderBuilder : ServiceProviderBuilderBase
{
    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddData();
        services.AddFileStorage();
        services.AddMessageQueue();
        services.AddOptions<YandexReverseImageSearchSettings>()
            .Configure(settings =>
            {
                var url = configuration["YANDEX_REVERSE_IMAGE_SEARCH_URL"]
                          ?? throw new InvalidOperationException(
                              "Required configuration YANDEX_REVERSE_IMAGE_SEARCH_URL not found");

                settings.ApiKey = configuration["YANDEX_REVERSE_IMAGE_SEARCH_API_KEY"]
                                  ?? throw new InvalidOperationException(
                                      "Required configuration YANDEX_REVERSE_IMAGE_SEARCH_API_KEY not found");

                settings.Url = new Uri(url);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddHttpClient<IYandexReverseImageSearchClient, YandexReverseImageSearchClient>();
    }
}