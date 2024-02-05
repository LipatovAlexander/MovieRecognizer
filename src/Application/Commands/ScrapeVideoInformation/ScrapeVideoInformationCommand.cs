namespace Application.Commands.ScrapeVideoInformation;

public record ScrapeVideoInformationCommand(Guid MovieRecognitionId) : ICommand;