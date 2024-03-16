using CloudFunctions;
using Data;
using MessageQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode;

namespace ReceiveVideoHandler;

public class ServiceProviderBuilder : ServiceProviderBuilderBase
{
    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddData();
        services.AddMessageQueue();
        services.AddSingleton<YoutubeClient>();
    }
}