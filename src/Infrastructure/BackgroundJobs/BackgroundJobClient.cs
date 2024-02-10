using Application.BackgroundJobs;
using Domain.Entities;
using Hangfire.States;
using IHangfireBackgroundJobClient = Hangfire.IBackgroundJobClient;
using Job = Hangfire.Common.Job;

namespace Infrastructure.BackgroundJobs;

public class BackgroundJobClient(IHangfireBackgroundJobClient hangfireBackgroundJobClient) : IBackgroundJobClient
{
    private readonly IHangfireBackgroundJobClient _hangfireBackgroundJobClient = hangfireBackgroundJobClient;
    
    public string Enqueue<TBackgroundJob>(MovieRecognition movieRecognition) where TBackgroundJob : IBackgroundJob
    {
        return _hangfireBackgroundJobClient.Create(
            Job.FromExpression<TBackgroundJob>(job => job.HandleAsync(movieRecognition.Id, CancellationToken.None)),
            new EnqueuedState());
    }

    public string ContinueWith<TBackgroundJob>(string parentJobId, MovieRecognition movieRecognition) where TBackgroundJob : IBackgroundJob
    {
        return _hangfireBackgroundJobClient.Create(
            Job.FromExpression<TBackgroundJob>(job => job.HandleAsync(movieRecognition.Id, CancellationToken.None)),
            new AwaitingState(parentJobId));
    }
}