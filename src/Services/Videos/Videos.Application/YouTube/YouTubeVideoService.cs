using OneOf;
using OneOf.Types;
using VideoLibrary;
using VideoLibrary.Exceptions;
using Video = Domain.Video;

namespace Videos.Application.YouTube;

public interface IYouTubeVideoService : IVideoService<YouTubeSource>;

public sealed class YouTubeVideoService(Client<YouTubeVideo> client) : IYouTubeVideoService
{
    public async Task<OneOf<Video, NotFound>> FindAsync(YouTubeSource source, CancellationToken cancellationToken)
    {
        try
        {
            var video = await client.GetVideoAsync(source.Uri.ToString());
        
            return new Video
            {
                Title = video.Title,
                Uri = new Uri(video.Uri),
                FileExtension = video.FileExtension,
                Author = video.Info.Author,
                Length = video.Info.LengthSeconds,
                ContentLength = video.ContentLength
            };
        }
        catch (UnavailableStreamException)
        {
            return new NotFound();
        }
    }
}
