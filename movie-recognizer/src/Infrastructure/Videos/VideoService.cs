using Application;
using Application.Files;
using Application.Videos;
using Microsoft.EntityFrameworkCore;
using OneOf;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using Video = Domain.Entities.Video;

namespace Infrastructure.Videos;

public class VideoService(YoutubeClient youtubeClient, IApplicationDbContext dbContext) : IVideoService
{
    private readonly YoutubeClient _youtubeClient = youtubeClient;
    private readonly IApplicationDbContext _dbContext = dbContext;
    
    public async Task<OneOf<Video, VideoNotFound, WebSiteNotSupported>> FindAsync(Uri videoUrl, CancellationToken cancellationToken)
    {
        try
        {
            var videoId = VideoId.TryParse(videoUrl.ToString())
                          ?? throw new WebSiteNotSupportedException();

            var existingVideo = await _dbContext.Videos
                .FirstOrDefaultAsync(Video.WithExternalId(videoId.Value), cancellationToken);

            if (existingVideo is not null)
            {
                return existingVideo;
            }

            var youtubeVideo = await _youtubeClient.Videos.GetAsync(videoId, cancellationToken);

            var title = youtubeVideo.Title;
            var author = youtubeVideo.Author.ChannelTitle;
            var duration = youtubeVideo.Duration
                ?? throw new InvalidOperationException("Video duration is null");

            return new Video(videoId.Value, title, author, duration);
        }
        catch (WebSiteNotSupportedException)
        {
            return new WebSiteNotSupported();
        }
        catch (VideoUnavailableException)
        {
            return new VideoNotFound();
        }
    }

    public async Task<TempFile> DownloadAsync(Video video, CancellationToken cancellationToken)
    {
        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(video.ExternalId, cancellationToken);

        var stream = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

        var tempFile = new TempFile(stream.Container.Name);
        await _youtubeClient.Videos.Streams.DownloadAsync(stream, tempFile.FilePath, cancellationToken: cancellationToken);

        return tempFile;
    }
}
