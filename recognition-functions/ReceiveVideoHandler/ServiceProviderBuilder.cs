using CloudFunctions;
using Data.Extensions;
using MessageQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proxy;
using YoutubeExplode;

namespace ReceiveVideoHandler;

public class ServiceProviderBuilder : ServiceProviderBuilderBase
{
    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddData();
        services.AddMessageQueue();
        services.AddProxyHttpClient();
        services.AddSingleton(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("proxy");
            return new YoutubeClient(httpClient);
        });
    }
}