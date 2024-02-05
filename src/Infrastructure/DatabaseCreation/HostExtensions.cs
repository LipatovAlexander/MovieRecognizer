using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DatabaseCreation;

public static class HostExtensions
{
    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider, string databaseName, int interval = 200, int maxRetries = 20)
    {
        var databaseCreator = ActivatorUtilities.CreateInstance<DatabaseCreator>(serviceProvider);
        await databaseCreator.EnsureDatabaseCreatedAsync(databaseName, interval, maxRetries);
    }
}