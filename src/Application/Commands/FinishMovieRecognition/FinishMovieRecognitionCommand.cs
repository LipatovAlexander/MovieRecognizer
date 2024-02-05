namespace Application.Commands.FinishMovieRecognition;

public record FinishMovieRecognitionCommand(Guid MovieRecognitionId) : ICommand;