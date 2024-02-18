using File = Domain.Entities.File;

namespace Application.Files;

public interface IFileStorage
{
    Task<File> SaveAsync(TempFile tempFile, CancellationToken cancellationToken);
}