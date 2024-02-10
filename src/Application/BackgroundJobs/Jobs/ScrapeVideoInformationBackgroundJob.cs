namespace Application.BackgroundJobs.Jobs;

public class ScrapeVideoInformationBackgroundJob : IBackgroundJob
{
    public Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Scraping video");
        return Task.CompletedTask;
    }
}