namespace Application.Commands.RecognizeMovie;

public record RecognizeMovieCommand(Guid MovieRecognitionId) : ICommand;