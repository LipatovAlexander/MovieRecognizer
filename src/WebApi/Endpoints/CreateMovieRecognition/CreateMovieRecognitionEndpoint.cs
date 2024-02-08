using Application;
using Application.Jobs;
using Domain.Entities;
using Infrastructure.WebApi.ApiResponses;
using Infrastructure.WebApi.Endpoints;
using Infrastructure.WebApi.Validation;

namespace WebApi.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionEndpoint : IEndpoint<
    SuccessResponse<MovieRecognition>,
    CreateMovieRecognitionRequest,
    IApplicationDbContext,
    IBackgroundJobClient>
{
    public static async Task<SuccessResponse<MovieRecognition>> HandleAsync(
        [AsParameters, Validate] CreateMovieRecognitionRequest request,
        IApplicationDbContext dbContext,
        IBackgroundJobClient backgroundJobClient,
        CancellationToken cancellationToken)
    {
        var movieRecognition = new MovieRecognition(request.VideoUrl);

        dbContext.MovieRecognitions.Add(movieRecognition);
        await dbContext.SaveChangesAsync(cancellationToken);

        var movieRecognitionContext = new MovieRecognitionContext(movieRecognition.Id);
        backgroundJobClient.Enqueue<StartMovieRecognitionBackgroundJob, MovieRecognitionContext>(movieRecognitionContext);
        
        return Responses.Success(movieRecognition);
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}