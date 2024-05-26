using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Mappers;
using Api.Models;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.GetTopRecognizedMovies;

public class GetTopRecognizedMoviesEndpoint : IEndpoint<
	Ok<SuccessResponse<TopRecognizedTitleDto[]>>,
	GetTopRecognizedMoviesRequest,
	IDatabaseContext>
{
	public static async Task<Ok<SuccessResponse<TopRecognizedTitleDto[]>>> HandleAsync(
		[AsParameters] GetTopRecognizedMoviesRequest request,
		IDatabaseContext databaseContext,
		CancellationToken cancellationToken)
	{
		var topRecognizedTitles = await databaseContext.MovieRecognitions.GetTopRecognizedMoviesAsync(request.Limit);
		var topRecognizedTitleDtos = topRecognizedTitles.Select(x => x.ToDto()).ToArray();
		return TypedResults.Ok(Responses.Success(topRecognizedTitleDtos));
	}

	public static void AddRoute(IEndpointRouteBuilder builder)
	{
		builder.MapGet("recognition/top", HandleAsync)
			.RequireApiKeyAuthentication();
	}
}