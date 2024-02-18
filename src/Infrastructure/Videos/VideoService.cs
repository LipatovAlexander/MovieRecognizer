using Application.Videos;
using OneOf;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos;
using Video = Domain.Entities.Video;

namespace Infrastructure.Videos;

public class VideoService(YoutubeClient youtubeClient) : IVideoService
{
    private readonly YoutubeClient _youtubeClient = youtubeClient;
    
    public async Task<OneOf<Video, VideoNotFound, WebSiteNotSupported>> FindAsync(Uri videoUrl, CancellationToken cancellationToken)
    {
        try
        {
            var videoId = VideoId.TryParse(videoUrl.ToString())
                          ?? throw new WebSiteNotSupportedException();

            var video = await _youtubeClient.Videos.GetAsync(videoId, cancellationToken);

            var title = video.Title;
            var author = video.Author.ChannelTitle;
            var duration = video.Duration;

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
