namespace Application.Jobs;

public class ScrapeVideoInformationBackgroundJob : IBackgroundJob<MovieRecognitionContext>
{
    public Task HandleAsync(MovieRecognitionContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Scraping video");
        return Task.CompletedTask;
    }
}