using Application.Commands.ExtractFrames;
using Application.Commands.FinishMovieRecognition;
using Application.Commands.RecognizeMovie;
using Application.Commands.ScrapeVideoInformation;
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
        var scrapeVideoInformationJobId = _backgroundJobClient.Enqueue<ICommandHandler<ScrapeVideoInformationCommand>>(
            handler => handler.HandleAsync(scrapeVideoInformationCommand, CancellationToken.None));

        var extractFramesCommand = new ExtractFramesCommand(movieRecognition.Id);
        var extractFramesJobId = _backgroundJobClient.ContinueJobWith<ICommandHandler<ExtractFramesCommand>>(
            scrapeVideoInformationJobId,
            handler => handler.HandleAsync(extractFramesCommand, CancellationToken.None));

        var recognizeMovieCommand = new RecognizeMovieCommand(movieRecognition.Id);
        var recognizeMovieJobId = _backgroundJobClient.ContinueJobWith<ICommandHandler<RecognizeMovieCommand>>(
            extractFramesJobId,
            handler => handler.HandleAsync(recognizeMovieCommand, CancellationToken.None));

        var finishMovieRecognitionCommand = new FinishMovieRecognitionCommand(movieRecognition.Id);
        _backgroundJobClient.ContinueJobWith<ICommandHandler<FinishMovieRecognitionCommand>>(
            recognizeMovieJobId,
            handler => handler.HandleAsync(finishMovieRecognitionCommand, CancellationToken.None));
    }
}