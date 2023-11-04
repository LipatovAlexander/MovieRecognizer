using Domain;
using OneOf;
using OneOf.Types;

namespace Videos.Application;

public interface IVideoService<in TSource> where TSource : class, ISource
{
    Task<OneOf<Video, NotFound>> FindAsync(TSource source, CancellationToken cancellationToken);
}
