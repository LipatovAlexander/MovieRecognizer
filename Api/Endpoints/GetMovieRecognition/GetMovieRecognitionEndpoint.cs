using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Mappers;
using Api.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionEndpoint : IEndpoint<
    Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>,
    GetMovieRecognitionRequest,
    IMovieRecognitionRepository>
{
    public static async Task<Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>> HandleAsync(
        [AsParameters] GetMovieRecognitionRequest request,
        IMovieRecognitionRepository repository,
        CancellationToken cancellationToken)
    {
        var movieRecognition = await repository.GetAsync(request.Id);

        if (movieRecognition is null)
        {
            return TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound));
        }

        return TypedResults.Ok(Responses.Success(movieRecognition.ToDto()));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognition/{id:guid}", HandleAsync);
    }
}