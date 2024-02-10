namespace Application.BackgroundJobs;

public interface IBackgroundJob
{
    static abstract string Type { get; }
    
    Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken);
}