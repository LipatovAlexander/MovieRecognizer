using Application;
using Application.BackgroundJobs;
using Domain.Entities;
using Hangfire.States;
using Microsoft.EntityFrameworkCore;
using IHangfireBackgroundJobClient = Hangfire.IBackgroundJobClient;
using HangfireJob = Hangfire.Common.Job;

namespace Infrastructure.BackgroundJobs;

public class BackgroundJobClient(IHangfireBackgroundJobClient hangfireBackgroundJobClient, IApplicationDbContext dbContext) : IBackgroundJobClient
{
    private readonly IHangfireBackgroundJobClient _hangfireBackgroundJobClient = hangfireBackgroundJobClient;
    private readonly IApplicationDbContext _dbContext = dbContext;
    
    public async Task<string> EnqueueAsync<TBackgroundJob>(MovieRecognition movieRecognition, CancellationToken cancellationToken)
        where TBackgroundJob : IBackgroundJob
    {
        var jobId = _hangfireBackgroundJobClient.Create(
            HangfireJob.FromExpression<TBackgroundJob>(job => job.HandleAsync(movieRecognition.Id, CancellationToken.None)),
            new EnqueuedState());

        var job = new Job(jobId, TBackgroundJob.Type)
        {
            MovieRecognition = movieRecognition
        };

        _dbContext.Jobs.Add(job);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return jobId;
    }

    public async Task<string> ContinueWithAsync<TBackgroundJob>(string parentJobId, MovieRecognition movieRecognition, CancellationToken cancellationToken)
        where TBackgroundJob : IBackgroundJob
    {
        var parentJob = await _dbContext.Jobs
            .FirstOrDefaultAsync(Job.WithExternalId(parentJobId), cancellationToken);

        if (parentJob is null)
        {
            throw new InvalidOperationException("Parent job not found");
        }
        
        var jobId = _hangfireBackgroundJobClient.Create(
            HangfireJob.FromExpression<TBackgroundJob>(job => job.HandleAsync(movieRecognition.Id, CancellationToken.None)),
            new AwaitingState(parentJobId));
        
        var job = new Job(jobId, TBackgroundJob.Type)
        {
            MovieRecognition = movieRecognition,
            ParentJob = parentJob
        };

        _dbContext.Jobs.Add(job);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return jobId;
    }
}