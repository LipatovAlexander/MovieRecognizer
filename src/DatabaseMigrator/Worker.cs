using FluentMigrator.Runner;

namespace DatabaseMigrator;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory serviceScopeFactory,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        if (migrationRunner.HasMigrationsToApplyUp())
        {
            _logger.LogInformation("Applying migrations");

            migrationRunner.MigrateUp();
        }
        else
        {
            _logger.LogInformation("The database is already up to date");
        }
        
        _hostApplicationLifetime.StopApplication();
        return Task.CompletedTask;
    }
}