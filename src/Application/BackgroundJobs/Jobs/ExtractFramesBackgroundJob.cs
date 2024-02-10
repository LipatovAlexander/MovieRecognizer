namespace Application.BackgroundJobs.Jobs;

public class ExtractFramesBackgroundJob : IBackgroundJob
{
    public static string Type => "ExtractFrames";

    public Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Extracting frames");
        return Task.CompletedTask;
    }
}