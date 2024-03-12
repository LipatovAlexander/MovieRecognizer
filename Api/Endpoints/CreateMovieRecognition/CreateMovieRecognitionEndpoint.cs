using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Infrastructure.Validation;
using Api.Mappers;
using Api.Models;
using Data.Repositories;
using Domain;
using MessageQueue;
using MessageQueue.Messages;

namespace Api.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionEndpoint : IEndpoint<
    SuccessResponse<MovieRecognitionDto>,
    CreateMovieRecognitionRequest,
    IMovieRecognitionRepository,
    IMessageQueueClient>
{
    public static async Task<SuccessResponse<MovieRecognitionDto>> HandleAsync(
        [AsParameters, Validate] CreateMovieRecognitionRequest request,
        IMovieRecognitionRepository repository,
        IMessageQueueClient messageQueueClient,
        CancellationToken cancellationToken)
    {
        var movieRecognition = new MovieRecognition(request.VideoUrl);

        await repository.SaveAsync(movieRecognition);

        await messageQueueClient.SendAsync(new ReceiveVideoMessage(movieRecognition.Id), cancellationToken);

        return Responses.Success(movieRecognition.ToDto());
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}