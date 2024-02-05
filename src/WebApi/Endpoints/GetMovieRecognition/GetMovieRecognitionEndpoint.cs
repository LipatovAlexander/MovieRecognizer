using Application;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApiExtensions.ApiResponses;
using WebApiExtensions.Endpoints;

namespace WebApi.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionEndpoint : IEndpoint<
    Results<Ok<SuccessResponse<MovieRecognition>>, NotFound<ErrorResponse>>,
    GetMovieRecognitionRequest,
    IApplicationDbContext>
{
    public static async Task<Results<Ok<SuccessResponse<MovieRecognition>>, NotFound<ErrorResponse>>> HandleAsync(
        [AsParameters] GetMovieRecognitionRequest request,
        IApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var recognitionRequest = await dbContext.MovieRecognitions
            .FirstOrDefaultAsync(MovieRecognition.WithId(request.Id), cancellationToken);

        if (recognitionRequest is null)
        {
            return TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound));
        }

        return TypedResults.Ok(Responses.Success(recognitionRequest));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognition/{id:guid}", HandleAsync);
    }
}