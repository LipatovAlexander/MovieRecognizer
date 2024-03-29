using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace ProcessVideoHandler;

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

    public async Task FunctionHandler(MessageQueueEvent request)
    {
        await _yandexDbService.InitializeAsync();

        var messages = request.GetMessages<ProcessVideoMessage>();

        foreach (var message in messages)
        {
            var video = await _databaseContext.Videos.GetAsync(message.VideoId);
            var movieRecognition = await _databaseContext.MovieRecognitions.GetAsync(video.MovieRecognitionId);

            movieRecognition.Status = MovieRecognitionStatus.Failed;
            movieRecognition.FailureMessage = "Could not process video";

            await _databaseContext.MovieRecognitions.SaveAsync(movieRecognition);
        }
    }
}