using Application.Files;
using Application.Images;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.BackgroundJobs.Jobs;

public class RecognizeMovieBackgroundJob(IApplicationDbContext dbContext, IImageSearchService imageSearchService, IFileStorage fileStorage) : IBackgroundJob
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IImageSearchService _imageSearchService = imageSearchService;
    private readonly IFileStorage _fileStorage = fileStorage;
    
    public static string Type => "RecognizeMovie";

    public async Task RunAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .Include(recognition => recognition.Video)
            .ThenInclude(video => video!.VideoFrames)
            .FirstOrDefaultAsync(Specification.ById<MovieRecognition>(movieRecognitionId), cancellationToken);
        
        if (movieRecognition is null)
        {
            throw new InvalidOperationException("MovieRecognition not found");
        }

        var frames = movieRecognition.Video?.VideoFrames
            ?? throw new InvalidOperationException("Video frames must be extracted");

        foreach (var frame in frames)
        {
            var url = _fileStorage.GetUrl(frame.ExternalId);
            var searchResponse = await _imageSearchService.SearchAsync(url, cancellationToken);

            var recognized = searchResponse.KnowledgeGraph.FirstOrDefault(x => x.Source == "Кинопоиск");
        }
    }
}