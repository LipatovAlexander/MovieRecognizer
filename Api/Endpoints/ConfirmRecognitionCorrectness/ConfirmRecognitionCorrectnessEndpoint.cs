using Api.Infrastructure;
using Api.Infrastructure.ApiResponses;
using Api.Mappers;
using Api.Models;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.ConfirmRecognitionCorrectness;

public class ConfirmRecognitionCorrectnessEndpoint : IEndpoint<
	Results<Ok<SuccessResponse<MovieRecognitionDto>>, BadRequest<ErrorResponse>>,
	ConfirmRecognitionCorrectnessRequest,
	IDatabaseContext>
{
	public static async Task<Results<Ok<SuccessResponse<MovieRecognitionDto>>, BadRequest<ErrorResponse>>> HandleAsync(
		ConfirmRecognitionCorrectnessRequest request,
		IDatabaseContext databaseContext,
		CancellationToken cancellationToken)
	{
		var movieRecognition = await databaseContext.MovieRecognitions.TryGetAsync(request.MovieRecognitionId);

		if (movieRecognition is null)
		{
			return TypedResults.BadRequest(Responses.Error(CommonErrorCodes.NotFound));
		}

		movieRecognition.RecognizedCorrectly = request.RecognizedCorrectly;
		await databaseContext.MovieRecognitions.SaveAsync(movieRecognition);

		return TypedResults.Ok(Responses.Success(movieRecognition.ToDto()));
	}

	public static void AddRoute(IEndpointRouteBuilder builder)
	{
		builder.MapPost("recognition/confirm", HandleAsync)
			.RequireApiKeyAuthentication();
	}
}