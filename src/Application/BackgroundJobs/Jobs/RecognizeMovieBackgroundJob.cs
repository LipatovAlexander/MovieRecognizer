namespace Application.BackgroundJobs.Jobs;

public class RecognizeMovieBackgroundJob : IBackgroundJob
{
    public Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Recognizing movie");
        return Task.CompletedTask;
    }
}