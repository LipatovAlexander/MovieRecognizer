using FluentMigrator.Runner;

namespace DatabaseMigrator;

public class Worker(
    IServiceScopeFactory serviceScopeFactory,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateUp();

        _hostApplicationLifetime.StopApplication();
        return Task.CompletedTask;
    }
}