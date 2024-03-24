using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Mappers;
using Api.Models;
using Data;
using Files;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionEndpoint : IEndpoint<
    Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>,
    GetMovieRecognitionRequest,
    IDatabaseContext,
    IFileStorage>
{
    public static async Task<Results<Ok<SuccessResponse<MovieRecognitionDto>>, NotFound<ErrorResponse>>> HandleAsync(
        [AsParameters] GetMovieRecognitionRequest request,
        IDatabaseContext databaseContext,
        IFileStorage fileStorage,
        CancellationToken cancellationToken)
    {
        var movieRecognition = await databaseContext.MovieRecognitions.TryGetAsync(request.Id);

        if (movieRecognition is null)
        {
            return TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound));
        }

        var dto = movieRecognition.ToDto();

        if (movieRecognition.VideoId is not null)
        {
            var video = await databaseContext.Videos.GetAsync(movieRecognition.VideoId.Value);

            var videoDto = video.ToDto();

            var videoFrames = await databaseContext.VideoFrames.ListAsync(video.Id);

            if (videoFrames.Count != 0)
            {
                var videoFramesDto = new List<VideoFrameDto>();

                foreach (var videoFrame in videoFrames)
                {
                    var fileUrl = fileStorage.GetUrl(videoFrame.ExternalId);
                    videoFramesDto.Add(videoFrame.ToDto(fileUrl));
                }

                videoDto.VideoFrames = videoFramesDto;
            }

            dto.Video = videoDto;
        }

        return TypedResults.Ok(Responses.Success(dto));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognition/{id:guid}", HandleAsync);
    }
}