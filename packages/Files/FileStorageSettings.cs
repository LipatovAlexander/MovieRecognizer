using System.ComponentModel.DataAnnotations;

namespace Files;

public class FileStorageSettings
{
    [Required] public required Uri PublicUrl { get; set; }

    [Required] public required string BucketName { get; set; }
}