using Application;
using Hangfire.Common;
using Hangfire.States;
using IBackgroundJobClient = Application.IBackgroundJobClient;
using IHangfireBackgroundJobClient = Hangfire.IBackgroundJobClient;

namespace Infrastructure.BackgroundJobs;

public class BackgroundJobClient(IHangfireBackgroundJobClient hangfireBackgroundJobClient) : IBackgroundJobClient
{
    private readonly IHangfireBackgroundJobClient _hangfireBackgroundJobClient = hangfireBackgroundJobClient;
    
    public string Enqueue<TBackgroundJob, TContext>(TContext context) where TBackgroundJob : IBackgroundJob<TContext>
    {
        return _hangfireBackgroundJobClient.Create(
            Job.FromExpression<TBackgroundJob>(job => job.HandleAsync(context, CancellationToken.None)),
            new EnqueuedState());
    }

    public string ContinueWith<TBackgroundJob, TContext>(string parentJobId, TContext context) where TBackgroundJob : IBackgroundJob<TContext>
    {
        return _hangfireBackgroundJobClient.Create(
            Job.FromExpression<TBackgroundJob>(job => job.HandleAsync(context, CancellationToken.None)),
            new AwaitingState(parentJobId));
    }
}