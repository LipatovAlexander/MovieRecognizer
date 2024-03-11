using System.Text.Json.Serialization;

namespace CloudFunctions.MessageQueue;

public class MessageQueueEventMetadata
{
    [JsonPropertyName("event_id")] public required string EventId { get; set; }

    [JsonPropertyName("event_type")] public required string EventType { get; set; }

    [JsonPropertyName("created_at")] public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("cloud_id")] public required string CloudId { get; set; }

    [JsonPropertyName("folder_id")] public required string FolderId { get; set; }
}