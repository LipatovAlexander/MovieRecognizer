using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode;
using YoutubeExplode.Videos;
using Video = Domain.Video;

namespace ReceiveVideoHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    private readonly IYandexDbService _yandexDbService;
    private readonly IDatabaseContext _databaseContext;
    private readonly YoutubeClient _youtubeClient;

    public Handler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _yandexDbService = services.GetRequiredService<IYandexDbService>();
        _databaseContext = services.GetRequiredService<IDatabaseContext>();
        _youtubeClient = services.GetRequiredService<YoutubeClient>();
    }

    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        await _yandexDbService.InitializeAsync();
        var messages = messageQueueEvent.GetMessages<ReceiveVideoMessage>();

        foreach (var message in messages)
        {
            var movieRecognition = await _databaseContext.MovieRecognitions.GetAsync(message.MovieRecognitionId);

            if (movieRecognition is null)
            {
                throw new InvalidOperationException("Movie recognition not found");
            }

            var videoId = VideoId.TryParse(movieRecognition.VideoUrl.ToString())
                          ?? throw new InvalidOperationException("Invalid video url");

            var youtubeVideo = await _youtubeClient.Videos.GetAsync(videoId);

            var title = youtubeVideo.Title;
            var author = youtubeVideo.Author.ChannelTitle;
            var duration = youtubeVideo.Duration
                           ?? throw new InvalidOperationException("Could not determine video duration");

            var video = new Video(videoId.Value, title, author, duration);

            await _databaseContext.Videos.SaveAsync(video);
            await _databaseContext.MovieRecognitions.SaveAsync(movieRecognition);
        }
    }
}