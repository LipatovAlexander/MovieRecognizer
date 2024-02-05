using Application;
using Domain;
using WebApiExtensions.ApiResponses;
using WebApiExtensions.Endpoints;
using WebApiExtensions.Validation;

namespace WebApi.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionEndpoint : IEndpoint<
    SuccessResponse<MovieRecognition>,
    CreateMovieRecognitionRequest,
    IApplicationDbContext>
{
    public static async Task<SuccessResponse<MovieRecognition>> HandleAsync(
        [AsParameters, Validate] CreateMovieRecognitionRequest request,
        IApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var recognitionRequest = new MovieRecognition(request.VideoUrl);

        dbContext.MovieRecognitions.Add(recognitionRequest);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Responses.Success(recognitionRequest);
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}