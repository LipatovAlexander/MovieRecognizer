using Domain.Entities;
using OneOf;
using OneOf.Types;
using VideoLibrary;
using VideoLibrary.Exceptions;
using Video = Domain.Entities.Video;

namespace Videos.Application.YouTube;

public sealed class YouTubeVideoService(Client<YouTubeVideo> client) : IVideoService
{
    public async Task<OneOf<Video, NotFound, UnsupportedSource>> FindAsync(Uri uri, CancellationToken cancellationToken)
    {
        try
        {
            var video = await client.GetVideoAsync(uri.ToString());

            return new Video
            {
                Title = video.Title,
                Uri = new Uri(await video.GetUriAsync()),
                FileExtension = video.FileExtension,
                Author = video.Info.Author,
                LengthSeconds = video.Info.LengthSeconds,
                Source = VideoSource.YouTube
            };
        }
        catch (UnavailableStreamException)
        {
            return new NotFound();
        }
        catch (ArgumentException)
        {
            return new UnsupportedSource();
        }
    }
}
