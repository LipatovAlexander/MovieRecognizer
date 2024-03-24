using System.Text.Json.Serialization;

namespace Api.Models;

public class VideoFrameDto
{
    [JsonPropertyName("timestamp")] public required TimeSpan Timestamp { get; set; }

    [JsonPropertyName("fileUrl")] public required Uri FileUrl { get; set; }
    
    [JsonPropertyName("processed")] public required bool Processed { get; set; }

    [JsonPropertyName("recognized_titles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyCollection<RecognizedTitleDto>? RecognizedTitles { get; set; }
}