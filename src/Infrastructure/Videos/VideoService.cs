using Application;
using Application.Videos;
using Microsoft.EntityFrameworkCore;
using OneOf;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos;
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
            var duration = youtubeVideo.Duration;

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
}
