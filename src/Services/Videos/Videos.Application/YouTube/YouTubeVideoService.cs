using OneOf;
using OneOf.Types;
using VideoLibrary;
using VideoLibrary.Exceptions;
using Video = Domain.Video;

namespace Videos.Application.YouTube;

public interface IYouTubeVideoService : IVideoService<YouTubeSource>;

public sealed class YouTubeVideoService(Client<YouTubeVideo> client) : IYouTubeVideoService
{
    public async Task<OneOf<Video, NotFound, Error<string>>> FindAsync(YouTubeSource source, CancellationToken cancellationToken)
    {
        try
        {
            var video = await client.GetVideoAsync(source.Uri.ToString());

            return new Video
            {
                Title = video.Title,
                Uri = new Uri(await video.GetUriAsync()),
                FileExtension = video.FileExtension,
                Author = video.Info.Author,
                LengthSeconds = video.Info.LengthSeconds
            };
        }
        catch (UnavailableStreamException)
        {
            return new NotFound();
        }
        catch (ArgumentException)
        {
            return new Error<string>("Uri is not a valid YouTube URI");
        }
    }
}
