using Application.Exceptions;
using Application.Videos;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.BackgroundJobs.Jobs;

public class ScrapeVideoInformationBackgroundJob(IApplicationDbContext dbContext, IVideoService videoService) : IBackgroundJob
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IVideoService _videoService = videoService;
    
    public static string Type => "ScrapeVideoInformation";

    public async Task RunAsync(Guid movieRecognitionId, CancellationToken cancellationToken)
    {
        var movieRecognition = await _dbContext.MovieRecognitions
            .Include(recognition => recognition.Video)
            .FirstOrDefaultAsync(Specification.ById<MovieRecognition>(movieRecognitionId), cancellationToken);

        if (movieRecognition is null)
        {
            throw new InvalidOperationException("MovieRecognition not found");
        }

        if (movieRecognition.Video is not null)
        {
            return;
        }

        var videoResult = await _videoService.FindAsync(movieRecognition.VideoUrl, cancellationToken);

        var video = videoResult.Match(
            v => v,
            _ => throw new VideoNotFoundException(),
            _ => throw new WebSiteNotSupportedException());

        movieRecognition.Video = video;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}