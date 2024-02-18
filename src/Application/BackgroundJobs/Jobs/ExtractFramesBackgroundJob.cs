using Application.Files;
using Application.Videos;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Application.BackgroundJobs.Jobs;

public class ExtractFramesBackgroundJob(
    IApplicationDbContext dbContext,
    IVideoService videoService,
    IVideoConverter videoConverter,
    IFileStorage fileStorage): IBackgroundJob
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IVideoService _videoService = videoService;
    private readonly IVideoConverter _videoConverter = videoConverter;
    private readonly IFileStorage _fileStorage = fileStorage;
    
    public static string Type => "ExtractFrames";

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

        var video = movieRecognition.Video;

        if (video == null || video.VideoFrames.Count != 0)
        {
            return;
        }

        using var videoFile = await _videoService.DownloadAsync(video, cancellationToken);

        var timestamp = TimeSpan.Zero;
        
        while (timestamp < video.Duration)
        {
            using var snapshot = await _videoConverter.SnapshotAsync(videoFile, timestamp, cancellationToken);
            await _fileStorage.UploadAsync(snapshot, snapshot.FileName, cancellationToken);
            
            var videoFrame = new VideoFrame(timestamp)
            {
                File = new File(snapshot.FileName)
            };
            
            video.VideoFrames.Add(videoFrame);
            _dbContext.VideoFrames.Add(videoFrame);
            
            timestamp += TimeSpan.FromSeconds(3);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}