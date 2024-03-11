namespace MessageQueue;

public interface IMessage
{
    static abstract Uri GetQueueUrl(MessageQueueOptions options);
}