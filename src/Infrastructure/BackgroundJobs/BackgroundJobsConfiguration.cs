using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IBackgroundJobClient = Application.BackgroundJobs.IBackgroundJobClient;

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

            GlobalJobFilters.Filters.Clear();
            config.UseFilter(new FailOnApplicationExceptionFilter { Order = 0 });
            config.UseFilter(new AutomaticRetryAttribute { Attempts = 3, OnlyOn = [typeof(BackgroundJobExecutionException)], DelaysInSeconds = [1, 2, 3], Order = 1 });
            config.UseFilter(new HandleFailedJobsFilter(sp.GetRequiredService<IServiceScopeFactory>()) { Order = 2 });
            config.UseFilter(new ContinuationsSupportAttribute { Order = 3 });
            config.UseFilter(new CaptureCultureAttribute { Order = 4 });
            config.UseFilter(new StatisticsHistoryAttribute { Order = 5 });
        });

        services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();
    }
}