using Application;
using Domain;
using Domain.Entities;
using Infrastructure.WebApi.ApiResponses;
using Infrastructure.WebApi.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionEndpoint : IEndpoint<
    Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>,
    GetMovieRecognitionRequest,
    IApplicationDbContext>
{
    public static async Task<Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>> HandleAsync(
        [AsParameters] GetMovieRecognitionRequest request,
        IApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var recognitionRequest = await dbContext.MovieRecognitions
            .Include(movieRecognition => movieRecognition.Video)
            .ThenInclude(video => video!.VideoFrames)
            .Include(movieRecognition => movieRecognition.Movie)
            .Include(movieRecognition => movieRecognition.Jobs)
            .ThenInclude(job => job.ParentJob)
            .FirstOrDefaultAsync(Specification.ById<MovieRecognition>(request.Id), cancellationToken);

        if (recognitionRequest is null)
        {
            return TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound));
        }

        return TypedResults.Ok(Responses.Success(recognitionRequest.ToDto()));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognition/{id:guid}", HandleAsync);
    }
}