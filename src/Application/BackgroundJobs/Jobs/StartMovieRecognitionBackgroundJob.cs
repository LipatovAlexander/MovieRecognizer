using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.BackgroundJobs.Jobs;

public class StartMovieRecognitionBackgroundJob(IApplicationDbContext dbContext, IBackgroundJobClient backgroundJobClient)
    : IBackgroundJob
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
    
    public async Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(Specification.ById<MovieRecognition>(movieRecognitionId), cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.InProgress;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var scrapeVideoInformationJobId = _backgroundJobClient
            .Enqueue<ScrapeVideoInformationBackgroundJob>(movieRecognition);
        var extractFramesJobId = _backgroundJobClient
            .ContinueWith<ExtractFramesBackgroundJob>(scrapeVideoInformationJobId, movieRecognition);
        var recognizeMovieJobId = _backgroundJobClient
            .ContinueWith<RecognizeMovieBackgroundJob>(extractFramesJobId, movieRecognition);
        _backgroundJobClient
            .ContinueWith<FinishMovieRecognitionBackgroundJob>(recognizeMovieJobId, movieRecognition);
    }
}