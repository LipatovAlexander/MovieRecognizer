using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Videos.Application.YouTube;
using WebApiExtensions.ApiResponses;
using WebApiExtensions.MinimalApi;

namespace Videos.API.Endpoints.YouTube;

public sealed class YouTubeEndpoint(IYouTubeVideoService videoService) : IEndpoint<YouTubeRequest, Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>>>
{
    public async Task<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>>> HandleAsync([AsParameters, Validate] YouTubeRequest request, CancellationToken cancellationToken)
    {
        var source = new YouTubeSource
        {
            Uri = new Uri(request.Uri)
        };

        var result = await videoService.FindAsync(source, cancellationToken);

        return result.Match<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>>>(
            video => TypedResults.Ok(Responses.Success(video)),
            _ => TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound)));
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("youtube", HandleAsync);
    }
}
