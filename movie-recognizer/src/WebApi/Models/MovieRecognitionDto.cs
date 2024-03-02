using Domain.Entities;

namespace WebApi.Models;

public class MovieRecognitionDto
{
    public required Guid Id { get; set; }
    
    public required Uri VideoUrl { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }

    public required MovieRecognitionStatus Status { get; set; }
    
    public required VideoDto? Video { get; set; }
    
    public required MovieDto? Movie { get; set; }

    public required ICollection<JobDto> Jobs { get; set; }
}