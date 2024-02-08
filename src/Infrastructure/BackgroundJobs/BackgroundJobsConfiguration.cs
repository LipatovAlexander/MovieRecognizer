using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IBackgroundJobClient = Application.IBackgroundJobClient;

namespace Infrastructure.BackgroundJobs;

public static class BackgroundJobsConfiguration
{
    public static void AddBackgroundJobs(this IServiceCollection services)
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

        services.AddSingleton<IBackgroundJobClient, BackgroundJobClient>();
    }
}