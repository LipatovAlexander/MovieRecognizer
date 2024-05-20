using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.GetMovieRecognitionHistory;

public class GetMovieRecognitionHistoryRequest
{
    [FromQuery(Name = "userId")]
    public required Guid UserId { get; set; }
}