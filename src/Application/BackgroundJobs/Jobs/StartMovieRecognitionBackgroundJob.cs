using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.BackgroundJobs.Jobs;

public class StartMovieRecognitionBackgroundJob(IApplicationDbContext dbContext, IBackgroundJobClient backgroundJobClient)
    : IBackgroundJob
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

    public static string Type => "StartMovieRecognition";

    public async Task RunAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(Specification.ById<MovieRecognition>(movieRecognitionId), cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.InProgress;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var scrapeVideoInformationJob = await _backgroundJobClient
            .EnqueueAsync<ScrapeVideoInformationBackgroundJob>(movieRecognition, cancellationToken);
        var extractFramesJob = await _backgroundJobClient
            .ContinueWithAsync<ExtractFramesBackgroundJob>(scrapeVideoInformationJob, movieRecognition, cancellationToken);
        var recognizeMovieJob = await _backgroundJobClient
            .ContinueWithAsync<RecognizeMovieBackgroundJob>(extractFramesJob, movieRecognition, cancellationToken);
        await _backgroundJobClient
            .ContinueWithAsync<FinishMovieRecognitionBackgroundJob>(recognizeMovieJob, movieRecognition, cancellationToken);
    }
}