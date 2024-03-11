﻿namespace MessageQueue.Messages;

public class ReceiveVideoMessage(Guid movieRecognitionId) : IMessage
{
    public Guid MovieRecognitionId { get; set; } = movieRecognitionId;

    public static Uri GetQueueUrl(MessageQueueOptions options)
    {
        return options.ReceiveVideoQueueUrl;
    }
}