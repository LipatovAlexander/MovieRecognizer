namespace Domain;

public sealed class Movie
{
    public required string Title { get; set; }
    public required string Year { get; set; }
    public required string[] Genres { get; set; }
    public required string[] Actors { get; set; }
    public required string Plot { get; set; }
    public required string Country { get; set; }
    public required Uri PosterUri { get; set; }
    public required MovieType Type { get; set; }
}

public enum MovieType
{
    Series,
    Movie,
    Episode
}
