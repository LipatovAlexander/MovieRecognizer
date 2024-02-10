namespace Application.BackgroundJobs.Jobs;

public class ScrapeVideoInformationBackgroundJob : IBackgroundJob
{
    public static string Type => "ScrapeVideoInformation";

    public Task RunAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Scraping video");
        return Task.CompletedTask;
    }
}