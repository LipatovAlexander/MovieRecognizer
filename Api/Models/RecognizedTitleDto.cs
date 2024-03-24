using System.Text.Json.Serialization;

namespace Api.Models;

public class RecognizedTitleDto
{
    [JsonPropertyName("title")] public required string Title { get; set; }

    [JsonPropertyName("subtitle")] public required string Subtitle { get; set; }

    [JsonPropertyName("description")] public required string Description { get; set; }

    [JsonPropertyName("source")] public required string Source { get; set; }

    [JsonPropertyName("link")] public required Uri Link { get; set; }

    [JsonPropertyName("thumbnail")] public required Uri Thumbnail { get; set; }
}