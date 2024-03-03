using Application.Files;
using Domain.Entities;
using OneOf;

namespace Application.Videos;

public interface IVideoService
{
    Task<OneOf<Video, VideoNotFound, WebSiteNotSupported>> FindAsync(Uri videoUrl, CancellationToken cancellationToken);
    Task<TempFile> DownloadAsync(Video video, CancellationToken cancellationToken);
}