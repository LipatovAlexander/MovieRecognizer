namespace Domain.Entities;

public class MovieRecognition(Uri videoUrl) : BaseEntity
{
    public Uri VideoUrl { get; set; } = videoUrl;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public MovieRecognitionStatus Status { get; set; } = MovieRecognitionStatus.Created;
    
    public Video? Video { get; set; }
    
    public Movie? Movie { get; set; }

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}

public enum MovieRecognitionStatus
{
    Created,
    InProgress,
    Failed,
    Succeeded,
    Cancelled
}