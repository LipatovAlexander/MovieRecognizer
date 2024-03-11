using System.Text.Json;
using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data.Repositories;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ReceiveVideoHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    private readonly IMovieRecognitionRepository _movieRecognitionRepository;
    private readonly ILogger<Handler> _logger;

    public Handler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _movieRecognitionRepository = services.GetRequiredService<IMovieRecognitionRepository>();
        _logger = services.GetRequiredService<ILogger<Handler>>();
    }

    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        var messages = messageQueueEvent.GetMessages<ReceiveVideoMessage>();

        foreach (var message in messages)
        {
            var movieRecognition = await _movieRecognitionRepository.GetAsync(message.MovieRecognitionId);
            _logger.LogInformation("Movie recognition: {MovieRecognition}", JsonSerializer.Serialize(movieRecognition));
        }
    }
}