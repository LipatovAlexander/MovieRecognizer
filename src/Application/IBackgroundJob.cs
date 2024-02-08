namespace Application;

public interface IBackgroundJob<in TContext>
{
    Task HandleAsync(TContext context, CancellationToken cancellationToken);
}