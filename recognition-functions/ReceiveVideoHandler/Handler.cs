using CloudFunctions;
using CloudFunctions.MessageQueue;

namespace ReceiveVideoHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        foreach (var message in messageQueueEvent.Messages)
        {
            Console.WriteLine(message.Details.Message.Body);
        }
    }
}