namespace Application.BackgroundJobs;

public interface IBackgroundJob
{
    Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken);
}