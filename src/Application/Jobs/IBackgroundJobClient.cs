namespace Application.Jobs;

public interface IBackgroundJobClient
{
    string Enqueue<TBackgroundJob, TContext>(TContext context) where TBackgroundJob : IBackgroundJob<TContext>;
    string ContinueWith<TBackgroundJob, TContext>(string parentJobId, TContext context) where TBackgroundJob : IBackgroundJob<TContext>;
}