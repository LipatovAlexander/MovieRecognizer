using Application.BackgroundJobs;
using Hangfire.Common;
using Hangfire.States;
using BackgroundJobs_IBackgroundJobClient = Application.BackgroundJobs.IBackgroundJobClient;
using IHangfireBackgroundJobClient = Hangfire.IBackgroundJobClient;

namespace Infrastructure.BackgroundJobs;

public class BackgroundJobClient(IHangfireBackgroundJobClient hangfireBackgroundJobClient) : BackgroundJobs_IBackgroundJobClient
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