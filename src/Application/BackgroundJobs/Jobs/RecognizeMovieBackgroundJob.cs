namespace Application.BackgroundJobs.Jobs;

public class RecognizeMovieBackgroundJob : IBackgroundJob<MovieRecognitionContext>
{
    public Task HandleAsync(MovieRecognitionContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Recognizing movie");
        return Task.CompletedTask;
    }
}