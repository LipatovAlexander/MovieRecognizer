namespace Application.Commands.ExtractFrames;

public class ExtractFramesCommandHandler : ICommandHandler<ExtractFramesCommand>
{
    public Task HandleAsync(ExtractFramesCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine("Extracting frames");
        return Task.CompletedTask;
    }
}