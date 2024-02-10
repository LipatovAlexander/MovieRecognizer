using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.BackgroundJobs.Jobs;

public class FinishMovieRecognitionBackgroundJob(IApplicationDbContext dbContext) : IBackgroundJob
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public static string Type => "FinishMovieRecognition";

    public async Task HandleAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(Specification.ById<MovieRecognition>(movieRecognitionId), cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.Succeeded;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}