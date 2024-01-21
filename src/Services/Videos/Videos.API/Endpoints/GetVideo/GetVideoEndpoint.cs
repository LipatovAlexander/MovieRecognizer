using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Videos.Application;
using WebApiExtensions.ApiResponses;
using WebApiExtensions.Endpoints;
using WebApiExtensions.Validation;

namespace Videos.API.Endpoints.GetVideo;

public sealed class GetVideoEndpoint : IEndpoint<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>, BadRequest<ErrorResponse>>, GetVideoRequest, IVideoFinder>
{
    public static async Task<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [AsParameters, Validate] GetVideoRequest request,
        IVideoFinder videoFinder,
        CancellationToken cancellationToken)
    {
        var result = await videoFinder.FindAsync(request.Uri, cancellationToken);

        return result.Match<Results<Ok<SuccessResponse<Video>>, NotFound<ErrorResponse>, BadRequest<ErrorResponse>>>(
            video => TypedResults.Ok(Responses.Success(video)),
            _ => TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound, "No video found at this URL")),
            _ => TypedResults.BadRequest(Responses.Error("unsupported_source")));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("video", HandleAsync)
            .AddValidationFilter();
    }
}
