using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Files;

public class FileStorageSettings
{
    public const string SectionName = "FileStorage";

    [Required]
    public required string BucketName { get; set; }
}