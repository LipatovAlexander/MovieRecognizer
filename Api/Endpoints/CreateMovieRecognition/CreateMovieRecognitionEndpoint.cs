using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Infrastructure.Validation;
using Api.Mappers;
using Api.Models;
using Data;
using Domain;
using MessageQueue;
using MessageQueue.Messages;
using Ydb.Sdk.Services.Table;

namespace Api.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionEndpoint : IEndpoint<
    SuccessResponse<MovieRecognitionDto>,
    CreateMovieRecognitionRequest,
    IDatabaseContext,
    IMessageQueueClient>
{
    public static async Task<SuccessResponse<MovieRecognitionDto>> HandleAsync(
        [AsParameters, Validate] CreateMovieRecognitionRequest request,
        IDatabaseContext databaseContext,
        IMessageQueueClient messageQueueClient,
        CancellationToken cancellationToken)
    {
        var movieRecognition = new MovieRecognition(request.VideoUrl);

        await databaseContext.ExecuteAsync(async session =>
        {
            await session.MovieRecognitions.SaveAsync(movieRecognition, TxControl.BeginSerializableRW().Commit());
        });

        await messageQueueClient.SendAsync(new ReceiveVideoMessage(movieRecognition.Id), cancellationToken);

        return Responses.Success(movieRecognition.ToDto());
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}