namespace Application.BackgroundJobs;

public interface IBackgroundJob<in TContext>
{
    Task HandleAsync(TContext context, CancellationToken cancellationToken);
}