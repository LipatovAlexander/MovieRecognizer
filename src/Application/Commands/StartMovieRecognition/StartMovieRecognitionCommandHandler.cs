using Application.Commands.ExtractFrames;
using Application.Commands.FinishMovieRecognition;
using Application.Commands.RecognizeMovie;
using Application.Commands.ScrapeVideoInformation;
using Application.Extensions;
using Domain.Entities;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.StartMovieRecognition;

public class StartMovieRecognitionCommandHandler(IApplicationDbContext dbContext, IBackgroundJobClient backgroundJobClient)
    : ICommandHandler<StartMovieRecognitionCommand>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
    
    public async Task HandleAsync(StartMovieRecognitionCommand command, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .FirstOrDefaultAsync(MovieRecognition.WithId(command.MovieRecognitionId), cancellationToken);

        if (movieRecognition is null)
        {
            return;
        }

        movieRecognition.Status = MovieRecognitionStatus.InProgress;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var scrapeVideoInformationCommand = new ScrapeVideoInformationCommand(movieRecognition.Id);
        var scrapeVideoInformationJobId = _backgroundJobClient.EnqueueCommand(scrapeVideoInformationCommand);
        
        var extractFramesCommand = new ExtractFramesCommand(movieRecognition.Id);
        var extractFramesJobId = _backgroundJobClient.ContinueWithCommand(scrapeVideoInformationJobId, extractFramesCommand);

        var recognizeMovieCommand = new RecognizeMovieCommand(movieRecognition.Id);
        var recognizeMovieJobId = _backgroundJobClient.ContinueWithCommand(extractFramesJobId, recognizeMovieCommand);

        var finishMovieRecognitionCommand = new FinishMovieRecognitionCommand(movieRecognition.Id);
        _backgroundJobClient.ContinueWithCommand(recognizeMovieJobId, finishMovieRecognitionCommand);
    }
}