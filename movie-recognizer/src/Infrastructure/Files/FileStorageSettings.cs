using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Files;

public class FileStorageSettings
{
    public const string SectionName = "FileStorage";

    [Required]
    public required string ServiceUrl { get; set; }
    
    [Required]
    public required string BucketName { get; set; }
    
    [Required]
    public required string AccessKey { get; set; }
    
    [Required]
    public required string SecretKey { get; set; }
}