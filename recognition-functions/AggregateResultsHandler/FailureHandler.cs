using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace AggregateResultsHandler;

public class FailureHandler : IHandler<MessageQueueEvent>
{
    private readonly IYandexDbService _yandexDbService;
    private readonly IDatabaseContext _databaseContext;

    public FailureHandler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _yandexDbService = services.GetRequiredService<IYandexDbService>();
        _databaseContext = services.GetRequiredService<IDatabaseContext>();
    }

    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        await _yandexDbService.InitializeAsync();

        var messages = messageQueueEvent.GetMessages<AggregateResultsMessage>();

        foreach (var message in messages)
        {
            var movieRecognition = await _databaseContext.MovieRecognitions.GetAsync(message.MovieRecognitionId);

            movieRecognition.Status = MovieRecognitionStatus.Failed;
            movieRecognition.FailureMessage = "Could not aggregate results";

            await _databaseContext.MovieRecognitions.SaveAsync(movieRecognition);
        }
    }
}