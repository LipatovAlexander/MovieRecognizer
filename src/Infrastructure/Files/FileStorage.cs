using Amazon.S3;
using Amazon.S3.Model;
using Application.Files;
using Microsoft.Extensions.Options;
using File = Domain.Entities.File;

namespace Infrastructure.Files;

public class FileStorage(IAmazonS3 amazonS3, IOptionsMonitor<FileStorageSettings> fileStorageSettings) : IFileStorage
{
    private readonly IAmazonS3 _amazonS3 = amazonS3;
    private readonly IOptionsMonitor<FileStorageSettings> _fileStorageSettings = fileStorageSettings;

    public async Task<File> UploadAsync(TempFile tempFile, CancellationToken cancellationToken)
    {
        var key = Path.GetFileName(tempFile.FilePath);
        
        var request = new PutObjectRequest
        {
            Key = key,
            BucketName = _fileStorageSettings.CurrentValue.BucketName,
            FilePath = tempFile.FilePath
        };

        await _amazonS3.PutObjectAsync(request, cancellationToken);

        return new File(key);
    }
}