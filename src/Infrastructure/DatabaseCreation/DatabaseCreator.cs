using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Infrastructure.DatabaseCreation;

public class DatabaseCreator(ILogger<DatabaseCreator> logger, IConfiguration configuration)
{
    private readonly ILogger<DatabaseCreator> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    
    public async Task EnsureDatabaseCreatedAsync(string databaseName, int interval, int maxRetries)
    {
        var connectionString = _configuration.GetConnectionString("postgres");
        await using var connection = new NpgsqlConnection(connectionString);
        
        var retry = 0;
        
        _logger.LogInformation("Creating database {Database} if not exists", databaseName);
        
        while (true)
        {
            try
            {
                await connection.OpenAsync();
                break;
            }
            catch (NpgsqlException)
            {
                if (retry < maxRetries)
                {
                    _logger.LogInformation("Retrying to connect {Retry} times", retry);
                    
                    retry++;
                    await Task.Delay(interval);
                    continue;
                }

                throw;
            }
        }

        await using var checkIfDatabaseExistsCommand = connection.CreateCommand();
        checkIfDatabaseExistsCommand.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}';";
        var checkIfDatabaseExistsResult = await checkIfDatabaseExistsCommand.ExecuteScalarAsync();

        if (checkIfDatabaseExistsResult is null)
        {
            
            await using var createDatabaseCommand = connection.CreateCommand();
            createDatabaseCommand.CommandText = $"CREATE DATABASE {databaseName};";
            await createDatabaseCommand.ExecuteNonQueryAsync();
            
            _logger.LogInformation("Database {Database} created", databaseName);
        }
        else
        {
            _logger.LogInformation("Database {Database} already existed", databaseName);
        }
    }
}