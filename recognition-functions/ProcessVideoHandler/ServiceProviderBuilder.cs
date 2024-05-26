using CloudFunctions;
using Data.Extensions;
using Files;
using MessageQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proxy;
using YoutubeExplode;

namespace ProcessVideoHandler;

public class ServiceProviderBuilder : ServiceProviderBuilderBase
{
    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddData();
        services.AddMessageQueue();
        services.AddFileStorage();
        services.AddProxyHttpClient();
        services.AddSingleton(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("proxy");
            return new YoutubeClient(httpClient);
        });
    }
}