using CliWrap;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Files;

public class NgrokBackgroundService(IOptions<FileStorageSettings> fileStorageSettings) : BackgroundService
{
    private readonly IOptions<FileStorageSettings> _fileStorageSettings = fileStorageSettings;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var serviceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL");

        await Cli.Wrap("ngrok")
            .WithArguments([
                "http",
                $"--domain={_fileStorageSettings.Value.PublicDomain}",
                serviceUrl
            ])
            .ExecuteAsync(stoppingToken);
    }
}