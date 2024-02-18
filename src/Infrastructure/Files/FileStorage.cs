using Amazon.S3;
using Amazon.S3.Model;
using Application.Files;
using File = Domain.Entities.File;

namespace Infrastructure.Files;

public class FileStorage(IAmazonS3 amazonS3) : IFileStorage
{
    private readonly IAmazonS3 _amazonS3 = amazonS3;

    private const string BucketName = "application";
    
    public async Task<File> SaveAsync(TempFile tempFile, CancellationToken cancellationToken)
    {
        var key = Path.GetFileName(tempFile.FilePath);
        
        var request = new PutObjectRequest
        {
            Key = key,
            BucketName = BucketName,
            FilePath = tempFile.FilePath
        };

        await _amazonS3.PutObjectAsync(request, cancellationToken);

        return new File(key);
    }
}