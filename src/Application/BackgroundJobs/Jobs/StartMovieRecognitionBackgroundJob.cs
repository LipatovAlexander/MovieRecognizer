using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.BackgroundJobs.Jobs;

public class StartMovieRecognitionBackgroundJob(IApplicationDbContext dbContext, IBackgroundJobClient backgroundJobClient)
    : IBackgroundJob<MovieRecognitionContext>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
    
    public async Task HandleAsync(MovieRecognitionContext context, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(Specification.ById<MovieRecognition>(context.MovieRecognitionId), cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.InProgress;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var scrapeVideoInformationJobId = _backgroundJobClient
            .Enqueue<ScrapeVideoInformationBackgroundJob, MovieRecognitionContext>(context);
        var extractFramesJobId = _backgroundJobClient
            .ContinueWith<ExtractFramesBackgroundJob, MovieRecognitionContext>(scrapeVideoInformationJobId, context);
        var recognizeMovieJobId = _backgroundJobClient
            .ContinueWith<RecognizeMovieBackgroundJob, MovieRecognitionContext>(extractFramesJobId, context);
        _backgroundJobClient
            .ContinueWith<FinishMovieRecognitionBackgroundJob, MovieRecognitionContext>(recognizeMovieJobId, context);
    }
}