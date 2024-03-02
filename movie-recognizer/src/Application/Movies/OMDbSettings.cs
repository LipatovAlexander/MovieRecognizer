using System.ComponentModel.DataAnnotations;

namespace Application.Movies;

public sealed class OMDbSettings
{
    public const string SectionName = "OMDb";

    [Required]
    public required string ApiKey { get; set; }
}
