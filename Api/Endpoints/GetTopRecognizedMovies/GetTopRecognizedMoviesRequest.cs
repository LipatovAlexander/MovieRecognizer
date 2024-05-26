using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.GetTopRecognizedMovies;

public class GetTopRecognizedMoviesRequest
{
	[FromQuery(Name = "limit")] public int Limit { get; set; }
}