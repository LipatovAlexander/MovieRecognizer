namespace Application.Commands.ScrapeVideoInformation;

public class ScrapeVideoInformationCommandHandler : ICommandHandler<ScrapeVideoInformationCommand>
{
    public Task HandleAsync(ScrapeVideoInformationCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine("Scraping video");
        return Task.CompletedTask;
    }
}