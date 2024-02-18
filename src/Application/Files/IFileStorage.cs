using File = Domain.Entities.File;

namespace Application.Files;

public interface IFileStorage
{
    Task<File> UploadAsync(TempFile tempFile, CancellationToken cancellationToken);
}