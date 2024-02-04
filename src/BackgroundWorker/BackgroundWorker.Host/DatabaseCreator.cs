using Npgsql;

namespace BackgroundWorker.Host;

public static class DatabaseCreator
{
    private const int MaxRetries = 5;

    public static async Task EnsureDatabaseCreatedAsync(this WebApplication app)
    {
        await using var dataSource = app.Services.GetRequiredService<NpgsqlDataSource>();
        await CreateDatabaseAsync(dataSource);
    }

    private static async Task CreateDatabaseAsync(NpgsqlDataSource dataSource)
    {
        NpgsqlConnection? npgsqlConnection = null;
        var retry = 0;

        while (npgsqlConnection is null)
        {
            try
            {
                npgsqlConnection = await dataSource.OpenConnectionAsync();
                await Task.Delay(100);
                retry++;
            }
            catch (NpgsqlException)
            {
                if (retry < MaxRetries)
                {
                    continue;
                }

                throw;
            }
        }

        await using var command = npgsqlConnection.CreateCommand();
        command.CommandText = """
                              DO
                              $do$
                              BEGIN
                                 IF EXISTS (SELECT FROM pg_database WHERE datname = 'hangfire') THEN
                                    RAISE NOTICE 'Database already exists';
                                 ELSE
                                    PERFORM dblink_exec('dbname=' || current_database(), 'CREATE DATABASE hangfire');
                                 END IF;
                              END
                              $do$;
                              """;
        await command.ExecuteNonQueryAsync();
    }
}