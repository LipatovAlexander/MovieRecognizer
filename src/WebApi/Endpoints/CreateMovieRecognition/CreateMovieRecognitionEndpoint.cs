using Application;
using Application.BackgroundJobs;
using Application.BackgroundJobs.Jobs;
using Domain.Entities;
using Infrastructure.WebApi.ApiResponses;
using Infrastructure.WebApi.Endpoints;
using Infrastructure.WebApi.Validation;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionEndpoint : IEndpoint<
    SuccessResponse<MovieRecognitionDto>,
    CreateMovieRecognitionRequest,
    IApplicationDbContext,
    IBackgroundJobClient>
{
    public static async Task<SuccessResponse<MovieRecognitionDto>> HandleAsync(
        [AsParameters, Validate] CreateMovieRecognitionRequest request,
        IApplicationDbContext dbContext,
        IBackgroundJobClient backgroundJobClient,
        CancellationToken cancellationToken)
    {
        var movieRecognition = new MovieRecognition(request.VideoUrl);

        dbContext.MovieRecognitions.Add(movieRecognition);
        await dbContext.SaveChangesAsync(cancellationToken);

        await backgroundJobClient.EnqueueAsync<StartMovieRecognitionBackgroundJob>(movieRecognition, cancellationToken);
        
        return Responses.Success(movieRecognition.ToDto());
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapPost("recognition", HandleAsync)
            .AddValidationFilter();
    }
}