using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Videos.Application.YouTube;
using WebApiExtensions.ApiResponses;
using WebApiExtensions.Endpoints;
using WebApiExtensions.Validation;

namespace Videos.API.Endpoints.YouTube;

public sealed class YouTubeEndpoint : IEndpoint<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>, BadRequest<ErrorResponse>>, YouTubeRequest, IYouTubeVideoService>
{
    public static async Task<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [AsParameters, Validate] YouTubeRequest request,
        IYouTubeVideoService videoService,
        CancellationToken cancellationToken)
    {
        var source = new YouTubeSource
        {
            Uri = new Uri(request.Uri)
        };

        var result = await videoService.FindAsync(source, cancellationToken);

        return result.Match<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>, BadRequest<ErrorResponse>>>(
            video => TypedResults.Ok(Responses.Success(video)),
            _ => TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound, "No video found at this URL")),
            error => TypedResults.BadRequest(Responses.Error(CommonErrorCodes.InvalidRequest, error.Value)));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("youtube", HandleAsync)
            .AddValidationFilter();
    }
}
