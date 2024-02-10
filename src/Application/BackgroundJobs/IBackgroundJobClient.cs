using Domain.Entities;

namespace Application.BackgroundJobs;

public interface IBackgroundJobClient
{
    string Enqueue<TBackgroundJob>(MovieRecognition movieRecognition) where TBackgroundJob : IBackgroundJob;
    string ContinueWith<TBackgroundJob>(string parentJobId, MovieRecognition movieRecognition) where TBackgroundJob : IBackgroundJob;
}