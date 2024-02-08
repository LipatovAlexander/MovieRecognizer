namespace Application.BackgroundJobs.Jobs;

public class ExtractFramesBackgroundJob : IBackgroundJob<MovieRecognitionContext>
{
    public Task HandleAsync(MovieRecognitionContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Extracting frames");
        return Task.CompletedTask;
    }
}