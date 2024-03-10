using System.ComponentModel.DataAnnotations;

namespace Data.YandexDb;

public class YandexDbSettings
{
    [Required] public required string Endpoint { get; set; }

    [Required] public required string Database { get; set; }
}