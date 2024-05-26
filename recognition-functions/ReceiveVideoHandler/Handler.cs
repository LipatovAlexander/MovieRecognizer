using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Data.Extensions;
using Domain;
using MessageQueue;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using Ydb.Sdk.Services.Table;
using YoutubeExplode;
using YoutubeExplode.Videos;
using Video = Domain.Video;

namespace ReceiveVideoHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    private readonly IYandexDbService _yandexDbService;
    private readonly IDatabaseContext _databaseContext;
    private readonly YoutubeClient _youtubeClient;
    private readonly IMessageQueueClient _messageQueueClient;

    public Handler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _yandexDbService = services.GetRequiredService<IYandexDbService>();
        _databaseContext = services.GetRequiredService<IDatabaseContext>();
        _youtubeClient = services.GetRequiredService<YoutubeClient>();
        _messageQueueClient = services.GetRequiredService<IMessageQueueClient>();
    }

    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        await _yandexDbService.InitializeAsync();

        var messages = messageQueueEvent.GetMessages<ReceiveVideoMessage>();

        foreach (var message in messages)
        {
            var movieRecognition = await _databaseContext.MovieRecognitions.GetAsync(message.MovieRecognitionId);

            movieRecognition.Status = MovieRecognitionStatus.InProgress;
            await _databaseContext.MovieRecognitions.SaveAsync(movieRecognition);

            var videoId = VideoId.TryParse(movieRecognition.VideoUrl.ToString())
                          ?? throw new InvalidOperationException("Invalid video url");

            var youtubeVideo = await _youtubeClient.Videos.GetAsync(videoId);

            var title = youtubeVideo.Title;
            var author = youtubeVideo.Author.ChannelTitle;
            var duration = youtubeVideo.Duration
                           ?? throw new InvalidOperationException("Could not determine video duration");

            if (duration > TimeSpan.FromMinutes(5))
            {
                movieRecognition.Status = MovieRecognitionStatus.Invalid;
                movieRecognition.FailureMessage = "Maximum video duration is 5 minutes";
                await _databaseContext.MovieRecognitions.SaveAsync(movieRecognition);
            }

            var video = new Video(movieRecognition.Id, videoId.Value, title, author, duration);

            await _databaseContext.ExecuteAsync(async session =>
            {
                var transaction = await session.Videos.SaveAsync(video, TxControl.BeginSerializableRW());
                transaction.EnsureNotNull();

                movieRecognition.VideoId = video.Id;
                await session.MovieRecognitions.SaveAsync(movieRecognition, TxControl.Tx(transaction).Commit());
            });

            await _messageQueueClient.SendAsync(new ProcessVideoMessage(video.Id));
        }
    }
}