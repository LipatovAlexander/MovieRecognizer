using Application;
using Application.Commands;
using Application.Commands.StartMovieRecognition;
using Application.Extensions;
using Domain.Entities;
using Hangfire;
using WebApiExtensions.ApiResponses;
using WebApiExtensions.Endpoints;
using WebApiExtensions.Validation;

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

        var startRecognitionCommand = new StartMovieRecognitionCommand(movieRecognition.Id);
        backgroundJobClient.EnqueueCommand(startRecognitionCommand);
        
        return Responses.Success(movieRecognition);
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}