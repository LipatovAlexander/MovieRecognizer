using System.ComponentModel.DataAnnotations;

namespace Movies.Application.OMDb;

public sealed class OMDbSettings
{
    public const string SectionName = "OMDb";

    [Required]
    public required string ApiKey { get; set; }
}
