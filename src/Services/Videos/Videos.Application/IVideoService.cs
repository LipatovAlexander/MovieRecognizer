using Domain;
using OneOf;
using OneOf.Types;

namespace Videos.Application;

public interface IVideoService
{
    Task<OneOf<Video, NotFound, UnsupportedSource>> FindAsync(Uri uri, CancellationToken cancellationToken);
}
