using Amazon.S3;
using Amazon.S3.Model;
using Application.Files;
using Microsoft.Extensions.Options;

namespace Infrastructure.Files;

public class FileStorage(IAmazonS3 amazonS3, IOptionsMonitor<FileStorageSettings> fileStorageSettings) : IFileStorage
{
    private readonly IAmazonS3 _amazonS3 = amazonS3;
    private readonly IOptionsMonitor<FileStorageSettings> _fileStorageSettings = fileStorageSettings;

    public async Task UploadAsync(TempFile tempFile, string key, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            Key = key,
            BucketName = _fileStorageSettings.CurrentValue.BucketName,
            FilePath = tempFile.FilePath
        };

        await _amazonS3.PutObjectAsync(request, cancellationToken);
    }

    public Uri GetPublicUrl(string key)
    {
        return new Uri(
            $"https://{_fileStorageSettings.CurrentValue.PublicDomain}/{_fileStorageSettings.CurrentValue.BucketName}/{key}");
    }
}