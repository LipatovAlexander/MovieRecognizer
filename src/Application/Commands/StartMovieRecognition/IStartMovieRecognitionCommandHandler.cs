namespace Application.Commands.StartMovieRecognition;

public interface IStartMovieRecognitionCommandHandler
{
    Task HandleAsync(StartMovieRecognitionCommand command, CancellationToken cancellationToken);
}