namespace Application.BackgroundJobs.Jobs;

public class RecognizeMovieBackgroundJob : IBackgroundJob
{
    public static string Type => "RecognizeMovie";

    public Task RunAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Recognizing movie");
        return Task.CompletedTask;
    }
}