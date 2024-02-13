using OneOf;
using OneOf.Types;
using VideoLibrary;
using VideoLibrary.Exceptions;
using Video = Domain.Entities.Video;

namespace Application.Videos;

public interface IVideoService
{
    Task<OneOf<Video, NotFound, WebSiteNotSupported>> FindAsync(Uri videoUrl, CancellationToken cancellationToken);
}

public class VideoService(Client<YouTubeVideo> youtubeVideoClient) : IVideoService
{
    private readonly Client<YouTubeVideo> _youtubeVideoClient = youtubeVideoClient;
    
    public async Task<OneOf<Video, NotFound, WebSiteNotSupported>> FindAsync(Uri videoUrl, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _youtubeVideoClient.GetVideoAsync(videoUrl.ToString());

            var title = video.Title;
            var author = video.Info.Author;
            var lengthSeconds = video.Info.LengthSeconds ?? throw new InvalidOperationException("Could not scrape video duration");
            var duration = TimeSpan.FromSeconds(lengthSeconds);
            var fileUrl = new Uri(await video.GetUriAsync());

            return new Video(title, author, duration, fileUrl, videoUrl);
        }
        catch (UnavailableStreamException)
        {
            return new NotFound();
        }
        catch (ArgumentException)
        {
            return new WebSiteNotSupported();
        }
    }
}
