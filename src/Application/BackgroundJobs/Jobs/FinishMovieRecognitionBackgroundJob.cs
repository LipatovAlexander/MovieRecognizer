using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.BackgroundJobs.Jobs;

public class FinishMovieRecognitionBackgroundJob(IApplicationDbContext dbContext) : IBackgroundJob<MovieRecognitionContext>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    
    public async Task HandleAsync(MovieRecognitionContext context, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(MovieRecognition.WithId(context.MovieRecognitionId), cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.Succeeded;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}