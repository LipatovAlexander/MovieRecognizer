using Domain.Entities;

namespace Application.BackgroundJobs;

public interface IBackgroundJobClient
{
    Task<string> EnqueueAsync<TBackgroundJob>(MovieRecognition movieRecognition, CancellationToken cancellationToken) where TBackgroundJob : IBackgroundJob;
    Task<string> ContinueWithAsync<TBackgroundJob>(string parentJobId, MovieRecognition movieRecognition, CancellationToken cancellationToken) where TBackgroundJob : IBackgroundJob;
}