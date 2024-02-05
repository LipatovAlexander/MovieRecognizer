using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class HangfireConfiguration
{
    public static void AddHangfireWithStorage(this IServiceCollection services)
    {
        services.AddHangfire((sp, config) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            
            config.UsePostgreSqlStorage(c =>
            {
                c.UseNpgsqlConnection(configuration.GetConnectionString("hangfire"));
            });

            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();
        });
    }
}