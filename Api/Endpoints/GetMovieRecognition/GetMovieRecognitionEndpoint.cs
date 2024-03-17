using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Mappers;
using Api.Models;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Ydb.Sdk.Services.Table;

namespace Api.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionEndpoint : IEndpoint<
    Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>,
    GetMovieRecognitionRequest,
    IDatabaseContext>
{
    public static async Task<Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>> HandleAsync(
        [AsParameters] GetMovieRecognitionRequest request,
        IDatabaseContext databaseContext,
        CancellationToken cancellationToken)
    {
        var movieRecognition = await databaseContext.ExecuteAsync(async session =>
        {
            var (movieRecognition, _) = await session.MovieRecognitions.TryGetAsync(
                request.Id,
                TxControl.BeginSerializableRW().Commit());

            return movieRecognition;
        });

        if (movieRecognition is null)
        {
            return TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound));
        }

        var dto = movieRecognition.ToDto();

        if (movieRecognition.VideoId is not null)
        {
            var video = await databaseContext.ExecuteAsync(async session =>
            {
                var (video, _) = await session.Videos.GetAsync(
                    movieRecognition.VideoId.Value,
                    TxControl.BeginSerializableRW().Commit());

                return video;
            });

            dto.Video = video.ToDto();
        }

        return TypedResults.Ok(Responses.Success(dto));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognition/{id:guid}", HandleAsync);
    }
}