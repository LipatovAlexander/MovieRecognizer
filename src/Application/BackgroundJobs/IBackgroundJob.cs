namespace Application.BackgroundJobs;

public interface IBackgroundJob
{
    static abstract string Type { get; }
    
    Task RunAsync(Guid movieRecognitionId, CancellationToken cancellationToken);
}