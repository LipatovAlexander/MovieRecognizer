using CloudFunctions;
using CloudFunctions.MessageQueue;
using MessageQueue.Messages;

namespace ReceiveVideoHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        var messages = messageQueueEvent.GetMessages<ReceiveVideoMessage>();

        foreach (var message in messages)
        {
            Console.WriteLine($"MovieRecognitionId: {message.MovieRecognitionId}");
        }
    }
}