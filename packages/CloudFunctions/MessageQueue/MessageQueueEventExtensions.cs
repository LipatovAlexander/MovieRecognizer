using System.Text.Json;
using MessageQueue;

namespace CloudFunctions.MessageQueue;

public static class MessageQueueEventExtensions
{
    public static IReadOnlyCollection<T> GetMessages<T>(this MessageQueueEvent messageQueueEvent)
        where T : IMessage
    {
        return messageQueueEvent.Messages
            .Select(messageQueueMessage => JsonSerializer.Deserialize<T>(messageQueueMessage.Details.Message.Body)!)
            .ToList();
    }
}