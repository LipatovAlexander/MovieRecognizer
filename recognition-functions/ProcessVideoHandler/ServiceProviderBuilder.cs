using CloudFunctions;
using Data;
using Files;
using MessageQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode;

namespace ProcessVideoHandler;

public class ServiceProviderBuilder : ServiceProviderBuilderBase
{
    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddData();
        services.AddMessageQueue();
        services.AddFileStorage();
        services.AddSingleton<YoutubeClient>();
    }
}