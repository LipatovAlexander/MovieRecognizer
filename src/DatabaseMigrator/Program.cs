using DatabaseCreation;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

await using var services = CreateServices();
using var scope = services.CreateScope();
await scope.ServiceProvider.EnsureDatabaseCreatedAsync("application");
var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

if (migrationRunner.HasMigrationsToApplyUp())
{
    logger.LogInformation("Applying migrations");

    migrationRunner.MigrateUp();
}
else
{
    logger.LogInformation("The database is already up to date");
}

return;


static ServiceProvider CreateServices()
{
    var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

    return new ServiceCollection()
        .AddSingleton<IConfiguration>(configuration)
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(configuration.GetConnectionString("application"))
            .ScanIn(typeof(Program).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddConsole())
        .BuildServiceProvider(false);
}