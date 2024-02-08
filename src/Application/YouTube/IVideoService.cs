using Domain.Entities;
using OneOf;
using OneOf.Types;

namespace Application.YouTube;

public interface IVideoService
{
    Task<OneOf<Video, NotFound, UnsupportedSource>> FindAsync(Uri uri, CancellationToken cancellationToken);
}
