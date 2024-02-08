using OneOf;
using OneOf.Types;

namespace Application.Videos;

public interface IVideoService
{
    Task<OneOf<Video, NotFound, UnsupportedSource>> FindAsync(Uri uri, CancellationToken cancellationToken);
}
