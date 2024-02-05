using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Movies.Application;
using WebApiExtensions.ApiResponses;
using WebApiExtensions.Endpoints;
using WebApiExtensions.Validation;

namespace Movies.API.Endpoints.FindMovie;

public sealed class FindMovieEndpoint : IEndpoint<Results<Ok<SuccessResponse<Movie>>, NotFound<ErrorResponse>>, FindMovieRequest, IMovieService>
{
    public static async Task<Results<Ok<SuccessResponse<Movie>>, NotFound<ErrorResponse>>> HandleAsync(
        [AsParameters, Validate] FindMovieRequest request,
        IMovieService movieService,
        CancellationToken cancellationToken)
    {
        var result = await movieService.FindMovieAsync(request.Title, cancellationToken);

        return result.Match<Results<Ok<SuccessResponse<Movie>>, NotFound<ErrorResponse>>>(
            movie => TypedResults.Ok(Responses.Success(movie)),
            _ => TypedResults.NotFound(Responses.Error(CommonErrorCodes.NotFound, "Movie not found")));
    }

    public static void AddRoute(IEndpointRouteBuilder builder)
    {
        builder.MapGet("movie", HandleAsync)
            .AddValidationFilter();
    }
}
