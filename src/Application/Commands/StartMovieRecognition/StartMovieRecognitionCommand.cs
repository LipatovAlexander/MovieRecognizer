namespace Application.Commands.StartMovieRecognition;

public record StartMovieRecognitionCommand(Guid MovieRecognitionId) : ICommand;