using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ydb.Sdk;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Yc;

namespace Data.YandexDb;

public interface IYandexDbService
{
    Task InitializeAsync();

    TableClient GetTableClient();
}

public class YandexDbService(IOptions<YandexDbSettings> settings, ILoggerFactory loggerFactory)
    : IYandexDbService, IDisposable
{
    private readonly IOptions<YandexDbSettings> _settings = settings;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    private Driver _driver = null!;

    public async Task InitializeAsync()
    {
        var metadataProvider = new MetadataProvider(loggerFactory: _loggerFactory);

        await metadataProvider.Initialize();

        var config = new DriverConfig(
            endpoint: _settings.Value.Endpoint,
            database: _settings.Value.Database,
            credentials: metadataProvider
        );

        _driver = new Driver(
            config: config,
            loggerFactory: _loggerFactory
        );

        await _driver.Initialize();
    }

    public TableClient GetTableClient()
    {
        return new TableClient(_driver);
    }

    public void Dispose()
    {
        _driver.Dispose();
    }
}