using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace RecognizeFrameHandler;

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

        var messages = messageQueueEvent.GetMessages<RecognizeFrameMessage>();

        foreach (var message in messages)
        {
            var videoFrame = await _databaseContext.VideoFrames.GetAsync(message.VideoFrameId);
            var video = await _databaseContext.Videos.GetAsync(videoFrame.VideoId);
            var movieRecognition = await _databaseContext.MovieRecognitions.GetAsync(video.MovieRecognitionId);

            movieRecognition.Status = MovieRecognitionStatus.Failed;
            movieRecognition.FailureMessage = "Could not recognize frame";

            await _databaseContext.MovieRecognitions.SaveAsync(movieRecognition);
        }
    }
}