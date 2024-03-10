using Microsoft.Extensions.Hosting;

namespace Data.YandexDb;

public class YandexDbServiceInitializer(IYandexDbService yandexDbService) : BackgroundService
{
    private readonly IYandexDbService _yandexDbService = yandexDbService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _yandexDbService.InitializeAsync();
    }
}