using System.Text.Json.Serialization;

namespace CloudFunctions.MessageQueue;

public class MessageQueueDetails
{
    [JsonPropertyName("queue_id")] public required string QueueId { get; set; }

    [JsonPropertyName("message")] public required MessageQueueMessageDetails Message { get; set; }
}