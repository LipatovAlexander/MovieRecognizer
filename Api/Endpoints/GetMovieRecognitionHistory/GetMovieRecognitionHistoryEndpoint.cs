using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Mappers;
using Api.Models;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.GetMovieRecognitionHistory;

public class GetMovieRecognitionHistoryEndpoint : IEndpoint<
    Ok<SuccessResponse<IReadOnlyCollection<MovieRecognitionDto>>>,
    GetMovieRecognitionHistoryRequest,
    IDatabaseContext>
{
    public static async Task<Ok<SuccessResponse<IReadOnlyCollection<MovieRecognitionDto>>>> HandleAsync(
        [AsParameters] GetMovieRecognitionHistoryRequest request,
        IDatabaseContext databaseContext,
        CancellationToken cancellationToken)
    {
        var movieRecognitions = await databaseContext.MovieRecognitions.ListByUserIdAsync(request.UserId);
        var movieRecognitionDto = movieRecognitions.Select(m => m.ToDto()).ToArray();

        return TypedResults.Ok(Responses.Success<IReadOnlyCollection<MovieRecognitionDto>>(movieRecognitionDto));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognition", HandleAsync)
            .RequireApiKeyAuthentication();
    }
}