using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using Ydb.Sdk.Services.Table;

namespace ReceiveVideoHandler;

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

        var messages = request.GetMessages<ReceiveVideoMessage>();

        foreach (var message in messages)
        {
            await _databaseContext.ExecuteAsync(async session =>
            {
                var (movieRecognition, transaction) = await session.MovieRecognitions.GetAsync(
                    message.MovieRecognitionId,
                    TxControl.BeginSerializableRW());

                transaction.EnsureNotNull();

                movieRecognition.Status = MovieRecognitionStatus.Failed;
                movieRecognition.FailureMessage = "Could not receive video from video url";

                await session.MovieRecognitions.SaveAsync(movieRecognition, TxControl.Tx(transaction).Commit());
            });
        }
    }
}