namespace Domain;

public class Video(Guid movieRecognitionId, string externalId, string title, string author, TimeSpan duration)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MovieRecognitionId { get; set; } = movieRecognitionId;
    public string ExternalId { get; set; } = externalId;
    public string Title { get; set; } = title;
    public string Author { get; set; } = author;
    public TimeSpan Duration { get; set; } = duration;
}