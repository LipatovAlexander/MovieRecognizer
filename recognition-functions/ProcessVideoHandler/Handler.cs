using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using Ydb.Sdk.Services.Table;

namespace ProcessVideoHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    private readonly IYandexDbService _yandexDbService;
    private readonly IDatabaseContext _databaseContext;

    public Handler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _yandexDbService = services.GetRequiredService<IYandexDbService>();
        _databaseContext = services.GetRequiredService<IDatabaseContext>();
    }

    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        await _yandexDbService.InitializeAsync();

        var messages = messageQueueEvent.GetMessages<ProcessVideoMessage>();

        foreach (var message in messages)
        {
            await _databaseContext.ExecuteAsync(async session =>
            {
                var (video, transaction) = await session.Videos.GetAsync(
                    message.VideoId,
                    TxControl.BeginSerializableRW());

                transaction.EnsureNotNull();

                Console.WriteLine($"Video: {video.Id}");
            });
        }
    }
}