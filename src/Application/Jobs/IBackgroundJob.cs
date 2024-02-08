namespace Application.Jobs;

public interface IBackgroundJob<in TContext>
{
    Task HandleAsync(TContext context, CancellationToken cancellationToken);
}