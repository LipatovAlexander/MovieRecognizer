using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionRequest
{
    [FromRoute(Name = "id")]
    public required Guid Id { get; set; }
}