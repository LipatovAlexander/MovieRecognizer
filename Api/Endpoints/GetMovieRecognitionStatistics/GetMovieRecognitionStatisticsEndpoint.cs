using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Mappers;
using Api.Models;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.GetMovieRecognitionStatistics;

public class GetMovieRecognitionStatisticsEndpoint : IEndpoint<
	Ok<SuccessResponse<MovieRecognitionStatisticsDto>>,
	IDatabaseContext>
{
	public static async Task<Ok<SuccessResponse<MovieRecognitionStatisticsDto>>> HandleAsync(
		IDatabaseContext databaseContext,
		CancellationToken cancellationToken)
	{
		var movieRecognitionStatistics = await databaseContext.MovieRecognitions.GetStatisticsAsync();

		return TypedResults.Ok(Responses.Success(movieRecognitionStatistics.ToDto()));
	}

	public static void AddRoute(IEndpointRouteBuilder builder)
	{
		builder.MapGet("recognition/statistics", HandleAsync)
			.RequireApiKeyAuthentication();
	}
}