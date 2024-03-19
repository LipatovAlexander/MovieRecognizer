namespace Files;

public interface IFileStorage
{
    Task UploadAsync(TempFile tempFile, string key, CancellationToken cancellationToken = default);

    Uri GetUrl(string key);
}