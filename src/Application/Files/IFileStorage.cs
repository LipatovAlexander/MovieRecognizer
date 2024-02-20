namespace Application.Files;

public interface IFileStorage
{
    Task UploadAsync(TempFile tempFile, string key, CancellationToken cancellationToken);
    Uri GetPublicUrl(string key);
}