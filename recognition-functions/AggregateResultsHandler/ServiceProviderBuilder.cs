using CloudFunctions;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AggregateResultsHandler;

public class ServiceProviderBuilder : ServiceProviderBuilderBase
{
    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddData();
    }
}