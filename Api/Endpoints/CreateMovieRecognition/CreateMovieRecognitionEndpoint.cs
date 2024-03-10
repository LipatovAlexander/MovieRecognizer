using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Infrastructure.Validation;
using Api.Mappers;
using Api.Models;
using Data.Repositories;
using Domain;

namespace Api.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionEndpoint : IEndpoint<
    SuccessResponse<MovieRecognitionDto>,
    CreateMovieRecognitionRequest,
    IMovieRecognitionRepository>
{
    public static async Task<SuccessResponse<MovieRecognitionDto>> HandleAsync(
        [AsParameters, Validate] CreateMovieRecognitionRequest request,
        IMovieRecognitionRepository repository,
        CancellationToken cancellationToken)
    {
        var movieRecognition = new MovieRecognition(
            Guid.NewGuid(),
            request.VideoUrl,
            MovieRecognitionStatus.Created,
            DateTime.UtcNow);

        await repository.SaveAsync(movieRecognition);

        return Responses.Success(movieRecognition.ToDto());
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}