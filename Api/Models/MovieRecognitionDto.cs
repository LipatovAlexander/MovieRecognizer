namespace Api.Models;

public class MovieRecognitionDto
{
    public required Guid Id { get; set; }
    
    public required Uri VideoUrl { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
}