namespace Application.Commands.RecognizeMovie;

public class RecognizeMovieCommandHandler : ICommandHandler<RecognizeMovieCommand>
{
    public Task HandleAsync(RecognizeMovieCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine("Recognizing movie");
        return Task.CompletedTask;
    }
}