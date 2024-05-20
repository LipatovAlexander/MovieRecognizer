using System.Text.Json.Serialization;
using Domain;

namespace Api.Models;

public class MovieRecognitionDto
{
    [JsonPropertyName("id")] public required Guid Id { get; set; }

    [JsonPropertyName("user_id")] public required Guid UserId { get; set; }

    [JsonPropertyName("video_url")] public required Uri VideoUrl { get; set; }

    [JsonPropertyName("created_at")] public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("status")] public required MovieRecognitionStatus Status { get; set; }

    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoDto? Video { get; set; }

    [JsonPropertyName("recognized_movie")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required RecognizedTitleDto? RecognizedMovie { get; set; }

    [JsonPropertyName("failure_message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required string? FailureMessage { get; set; }
}