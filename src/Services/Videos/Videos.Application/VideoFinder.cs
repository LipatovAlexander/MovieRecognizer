using Domain.Entities;
using OneOf;
using OneOf.Types;

namespace Videos.Application;

public interface IVideoFinder
{
    Task<OneOf<Video, NotFound, UnsupportedSource>> FindAsync(Uri uri, CancellationToken cancellationToken);
}

public sealed class VideoFinder(IEnumerable<IVideoService> videoServices) : IVideoFinder
{
    public async Task<OneOf<Video, NotFound, UnsupportedSource>> FindAsync(Uri uri, CancellationToken cancellationToken)
    {
        foreach (var videoService in videoServices)
        {
            var result = await videoService.FindAsync(uri, cancellationToken);
            if (result.IsT0)
            {
                return result.AsT0;
            }

            if (result.IsT1)
            {
                return new NotFound();
            }
        }

        return new UnsupportedSource();
    }
}
