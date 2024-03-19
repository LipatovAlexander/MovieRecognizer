using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using Ydb.Sdk.Services.Table;

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
            await _databaseContext.ExecuteAsync(async session =>
            {
                var (video, transaction) = await session.Videos.GetAsync(
                    message.VideoId,
                    TxControl.BeginSerializableRW());

                transaction.EnsureNotNull();

                (var movieRecognition, transaction) = await session.MovieRecognitions.GetAsync(
                    video.MovieRecognitionId,
                    TxControl.Tx(transaction));

                transaction.EnsureNotNull();

                movieRecognition.Status = MovieRecognitionStatus.Failed;
                movieRecognition.FailureMessage = "Could not process video";

                await session.MovieRecognitions.SaveAsync(movieRecognition, TxControl.Tx(transaction).Commit());
            });
        }
    }
}