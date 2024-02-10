namespace Application.BackgroundJobs.Jobs;

public class ExtractFramesBackgroundJob : IBackgroundJob
{
    public Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Extracting frames");
        return Task.CompletedTask;
    }
}