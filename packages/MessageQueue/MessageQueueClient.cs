using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace MessageQueue;

public interface IMessageQueueClient
{
    Task SendAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage;
}

public class MessageQueueClient(IAmazonSQS amazonSqs, IOptionsMonitor<MessageQueueOptions> options)
    : IMessageQueueClient
{
    private readonly IAmazonSQS _amazonSqs = amazonSqs;
    private readonly IOptionsMonitor<MessageQueueOptions> _options = options;

    public async Task SendAsync<T>(T message, CancellationToken cancellationToken) where T : IMessage
    {
        ArgumentNullException.ThrowIfNull(message);

        var request = new SendMessageRequest
        {
            QueueUrl = T.GetQueueUrl(_options.CurrentValue).ToString(),
            MessageBody = JsonSerializer.Serialize(message)
        };

        var response = await _amazonSqs.SendMessageAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException(
                $"Send message response is not successful. Status code: {response.HttpStatusCode}");
        }
    }
}