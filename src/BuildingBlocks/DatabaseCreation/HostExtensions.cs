using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DatabaseCreation;

public static class HostExtensions
{
    public static async Task EnsureDatabaseCreatedAsync(this IHost host, string databaseName, int interval = 200, int maxRetries = 10)
    {
        var databaseCreator = ActivatorUtilities.CreateInstance<DatabaseCreator>(host.Services);
        await databaseCreator.EnsureDatabaseCreatedAsync(databaseName, interval, maxRetries);
    }
}