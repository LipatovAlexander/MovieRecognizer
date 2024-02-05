using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.GetMovieRecognition;

public class GetMovieRecognitionRequest
{
    [FromRoute(Name = "id")]
    public required Guid Id { get; set; }
}