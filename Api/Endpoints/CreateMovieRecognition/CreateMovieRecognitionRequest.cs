namespace Api.Endpoints.CreateMovieRecognition;

public class CreateMovieRecognitionRequest
{
    public required Guid UserId { get; set; }
    public required Uri VideoUrl { get; set; }
}