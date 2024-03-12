using Domain;

namespace Api.Models;

public class MovieRecognitionDto
{
    public required Guid Id { get; set; }
    
    public required Uri VideoUrl { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required MovieRecognitionStatus Status { get; set; }
    
    public required Guid? VideoId { get; set; }
}