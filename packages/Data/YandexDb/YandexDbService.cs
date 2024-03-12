using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ydb.Sdk;
using Ydb.Sdk.Services.Query;
using Ydb.Sdk.Yc;

namespace Data.YandexDb;

public interface IYandexDbService
{
    Task InitializeAsync();

    QueryClient GetQueryClient();
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

    public QueryClient GetQueryClient()
    {
        return new QueryClient(_driver);
    }

    public void Dispose()
    {
        _driver.Dispose();
    }
}