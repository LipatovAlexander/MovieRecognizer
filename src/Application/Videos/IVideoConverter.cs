using Application.Files;

namespace Application.Videos;

public interface IVideoConverter
{
    Task<TempFile> SnapshotAsync(TempFile videoFile, TimeSpan timestamp, CancellationToken cancellationToken);
}