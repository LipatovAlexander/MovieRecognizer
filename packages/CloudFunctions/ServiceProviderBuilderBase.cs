using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudFunctions;

public abstract class ServiceProviderBuilderBase
{
    public ServiceProvider BuildServices()
    {
        var serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddLogging(b => b.AddConsole());

        ConfigureServices(serviceCollection, configuration);

        return serviceCollection.BuildServiceProvider();
    }

    protected abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}