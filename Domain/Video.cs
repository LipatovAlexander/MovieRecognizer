namespace Domain;

public class Video(Guid movieRecognitionId, string externalId, string title, string author, TimeSpan duration)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid MovieRecognitionId { get; } = movieRecognitionId;
    public string ExternalId { get; } = externalId;
    public string Title { get; } = title;
    public string Author { get; } = author;
    public TimeSpan Duration { get; } = duration;
}