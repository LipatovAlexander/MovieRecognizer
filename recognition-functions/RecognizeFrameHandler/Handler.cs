using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using Files;
using MessageQueue;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using Ydb.Sdk.Services.Table;

namespace RecognizeFrameHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    private readonly IYandexDbService _yandexDbService;
    private readonly IDatabaseContext _databaseContext;
    private readonly IFileStorage _fileStorage;
    private readonly IYandexReverseImageSearchClient _yandexReverseImageSearchClient;
    private readonly IMessageQueueClient _messageQueueClient;

    public Handler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _yandexDbService = services.GetRequiredService<IYandexDbService>();
        _databaseContext = services.GetRequiredService<IDatabaseContext>();
        _fileStorage = services.GetRequiredService<IFileStorage>();
        _yandexReverseImageSearchClient = services.GetRequiredService<IYandexReverseImageSearchClient>();
        _messageQueueClient = services.GetRequiredService<IMessageQueueClient>();
    }

    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        await _yandexDbService.InitializeAsync();

        var messages = messageQueueEvent.GetMessages<RecognizeFrameMessage>();

        foreach (var message in messages)
        {
            var videoFrame = await _databaseContext.VideoFrames.GetAsync(message.VideoFrameId);

            var fileUrl = _fileStorage.GetUrl(videoFrame.ExternalId);

            var reverseImageSearchResponse = await _yandexReverseImageSearchClient.SearchAsync(
                new YandexReverseImageSearchRequest
                {
                    ImageUrl = fileUrl
                });

            var recognizedTitles = reverseImageSearchResponse.KnowledgeGraph
                .Where(x => x.Source == "Кинопоиск")
                .Select(x => new RecognizedTitle(x.Title, x.Subtitle, x.Description, x.Source, x.Link, x.Thumbnail))
                .ToArray();

            await _databaseContext.ExecuteAsync(async session =>
            {
                Transaction? transaction = null;

                foreach (var recognizedTitle in recognizedTitles)
                {
                    var videoFrameRecognition =
                        new VideoFrameRecognition(videoFrame.VideoId, videoFrame.Id, recognizedTitle);

                    transaction = await session.VideoFrameRecognitions.SaveAsync(videoFrameRecognition, GetTxControl());
                }

                videoFrame.Processed = true;
                await session.VideoFrames.SaveAsync(videoFrame, GetTxControl().Commit());

                return;

                TxControl GetTxControl()
                {
                    return transaction is not null
                        ? TxControl.Tx(transaction)
                        : TxControl.BeginSerializableRW();
                }
            });

            var video = await _databaseContext.Videos.GetAsync(videoFrame.VideoId);
            var videoFrames = await _databaseContext.VideoFrames.ListAsync(video.Id);

            if (videoFrames.All(x => x.Processed))
            {
                await _messageQueueClient.SendAsync(new AggregateResultsMessage(video.MovieRecognitionId));
            }
        }
    }
}