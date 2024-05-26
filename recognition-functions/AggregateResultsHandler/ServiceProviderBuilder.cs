using CloudFunctions;
using Data.Extensions;
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