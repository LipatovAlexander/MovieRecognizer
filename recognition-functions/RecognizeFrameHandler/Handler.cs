using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using Files;
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

    public Handler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _yandexDbService = services.GetRequiredService<IYandexDbService>();
        _databaseContext = services.GetRequiredService<IDatabaseContext>();
        _fileStorage = services.GetRequiredService<IFileStorage>();
        _yandexReverseImageSearchClient = services.GetRequiredService<IYandexReverseImageSearchClient>();
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
                .Select(x => new RecognizedTitle
                {
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    Description = x.Description,
                    Source = x.Source,
                    Link = x.Link,
                    Thumbnail = x.Thumbnail
                })
                .ToArray();

            await _databaseContext.ExecuteAsync(async session =>
            {
                Transaction? transaction = null;

                foreach (var recognizedTitle in recognizedTitles)
                {
                    var videoFrameRecognition = new VideoFrameRecognition(videoFrame.Id, recognizedTitle);

                    var txControl = transaction is not null
                        ? TxControl.Tx(transaction)
                        : TxControl.BeginSerializableRW();

                    transaction = await session.VideoFrameRecognitions.SaveAsync(videoFrameRecognition, txControl);
                }

                transaction.EnsureNotNull();

                videoFrame.Processed = true;
                await session.VideoFrames.SaveAsync(videoFrame, TxControl.Tx(transaction).Commit());
            });
        }
    }
}