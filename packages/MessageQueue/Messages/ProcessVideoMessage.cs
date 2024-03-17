namespace MessageQueue.Messages;

public class ProcessVideoMessage(Guid videoId) : IMessage
{
    public Guid VideoId { get; set; } = videoId;

    public static Uri GetQueueUrl(MessageQueueOptions options)
    {
        return options.ProcessVideoQueueUrl;
    }
}