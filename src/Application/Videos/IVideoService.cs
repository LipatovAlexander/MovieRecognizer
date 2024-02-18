using Domain.Entities;
using OneOf;

namespace Application.Videos;

public interface IVideoService
{
    Task<OneOf<Video, VideoNotFound, WebSiteNotSupported>> FindAsync(Uri videoUrl, CancellationToken cancellationToken);
}