namespace Application.Commands.ExtractFrames;

public record ExtractFramesCommand(Guid MovieRecognitionId) : ICommand;