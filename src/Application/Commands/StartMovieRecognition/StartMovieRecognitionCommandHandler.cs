using Domain;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.StartMovieRecognition;

public class StartMovieRecognitionCommandHandler(IApplicationDbContext dbContext, IBackgroundJobClient backgroundJobClient)
    : IStartMovieRecognitionCommandHandler
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
    
    public async Task HandleAsync(StartMovieRecognitionCommand command, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(r => r.Id == command.MovieRecognitionId, cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.InProgress;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}