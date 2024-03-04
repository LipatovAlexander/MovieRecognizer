using Api.ApiResponses;
using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionEndpoint : IEndpoint<
    Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>,
    GetMovieRecognitionRequest>
{
    public static async Task<Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>> HandleAsync(
        [AsParameters] GetMovieRecognitionRequest request,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(Responses.Success(new MovieRecognitionDto
        {
            Id = request.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            VideoUrl = new Uri("https://example.com")
        }));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognition/{id:guid}", HandleAsync);
    }
}