using Recognizer.Application;
using WebApiExtensions.Endpoints;
using WebApiExtensions.Validation;

namespace Recognizer.API.Endpoints.RecognizeMovie;

public sealed class RecognizeMovieEndpoint : IEndpoint<RecognizeMovieResponse, RecognizeMovieRequest, IRecognitionService>
{
    public static async Task<RecognizeMovieResponse> HandleAsync(
        [AsParameters, Validate] RecognizeMovieRequest request,
        IRecognitionService recognitionService,
        CancellationToken cancellationToken)
    {
        return new RecognizeMovieResponse
        {
            Items = await recognitionService.RecognizeAsync(request.VideoUri, cancellationToken)
        };
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("recognize", HandleAsync)
            .AddValidationFilter();
    }
}
