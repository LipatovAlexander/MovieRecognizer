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

        var commandText = $"""
                      DO
                      $do$
                      BEGIN
                         IF EXISTS (SELECT FROM pg_database WHERE datname = '{databaseName}') THEN
                            RAISE NOTICE 'Database already exists';  -- optional
                         ELSE
                            PERFORM dblink_exec('dbname=' || current_database()  -- current db
                                              , 'CREATE DATABASE {databaseName}');
                         END IF;
                      END
                      $do$;
                      """;
        
        _logger.LogInformation("Creating database if not exists. Database name: {Database}", databaseName);
        
        while (true)
        {
            try
            {
                await connection.OpenAsync();
                await using var command = connection.CreateCommand();
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync();
                break;
            }
            catch (NpgsqlException)
            {
                if (retry < maxRetries)
                {
                    _logger.LogInformation("Retrying {Retry} times", retry);
                    
                    retry++;
                    await Task.Delay(interval);
                    continue;
                }

                throw;
            }
        }
        
        _logger.LogInformation("Database created or already existed. Database name: {Database}", databaseName);
    }
}