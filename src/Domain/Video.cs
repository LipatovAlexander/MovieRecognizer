namespace Domain;

public sealed class Video
{
    public required string Title { get; set; }
    public required Uri Uri { get; set; }
    public required string FileExtension { get; set; }
    public required string Author { get; set; }
    public required int? LengthSeconds { get; set; }
}
