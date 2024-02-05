using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.FinishMovieRecognition;

public class FinishMovieRecognitionCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<FinishMovieRecognitionCommand>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    
    public async Task HandleAsync(FinishMovieRecognitionCommand command, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(r => r.Id == command.MovieRecognitionId, cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.Succeeded;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}