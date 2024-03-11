using System.Text.Json;

namespace CloudFunctions.MessageQueue;

public static class MessageQueueEventExtensions
{
    public static IReadOnlyCollection<T> GetMessages<T>(this MessageQueueEvent messageQueueEvent)
    {
        return messageQueueEvent.Messages
            .Select(messageQueueMessage => JsonSerializer.Deserialize<T>(messageQueueMessage.Details.Message.Body)!)
            .ToList();
    }
}