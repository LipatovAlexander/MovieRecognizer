namespace MessageQueue.Messages;

public class RecognizeFrameMessage(Guid videoFrameId) : IMessage
{
    public Guid VideoFrameId { get; set; } = videoFrameId;

    public static Uri GetQueueUrl(MessageQueueOptions options)
    {
        return options.RecognizeFrameQueueUrl;
    }
}