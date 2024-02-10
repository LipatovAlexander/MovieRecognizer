using Domain.Entities;

namespace Application.BackgroundJobs;

public interface IBackgroundJobClient
{
    Task<Job> EnqueueAsync<TBackgroundJob>(MovieRecognition movieRecognition, CancellationToken cancellationToken) where TBackgroundJob : IBackgroundJob;
    Task<Job> ContinueWithAsync<TBackgroundJob>(Job parentJob, MovieRecognition movieRecognition, CancellationToken cancellationToken) where TBackgroundJob : IBackgroundJob;
}