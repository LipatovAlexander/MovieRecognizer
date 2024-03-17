using System.Text.Json.Serialization;

namespace Api.Models;

public class VideoDto
{
    [JsonPropertyName("external_id")] public required string ExternalId { get; set; }

    [JsonPropertyName("title")] public required string Title { get; set; }

    [JsonPropertyName("author")] public required string Author { get; set; }

    [JsonPropertyName("duration")] public required TimeSpan Duration { get; set; }
}