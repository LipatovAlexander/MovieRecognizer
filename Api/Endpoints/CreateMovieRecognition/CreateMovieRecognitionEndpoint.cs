using Api.ApiResponses;
using Api.Models;
using Api.Validation;

namespace Api.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionEndpoint : IEndpoint<
    SuccessResponse<MovieRecognitionDto>,
    CreateMovieRecognitionRequest>
{
    public static async Task<SuccessResponse<MovieRecognitionDto>> HandleAsync(
        [AsParameters, Validate] CreateMovieRecognitionRequest request,
        CancellationToken cancellationToken)
    {
        return Responses.Success(new MovieRecognitionDto
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            VideoUrl = request.VideoUrl
        });
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}