namespace Domain.Entities;

public class Movie(
    string imdbId,
    string title,
    string year,
    IReadOnlyCollection<string> genres,
    IReadOnlyCollection<string> actors,
    string plot,
    string country,
    Uri posterUrl,
    MovieType type) : BaseEntity
{
    public string ImdbId { get; set; } = imdbId;

    public string Title { get; set; } = title;

    public string Year { get; set; } = year;

    public IReadOnlyCollection<string> Genres { get; set; } = genres;

    public IReadOnlyCollection<string> Actors { get; set; } = actors;

    public string Plot { get; set; } = plot;

    public string Country { get; set; } = country;

    public Uri PosterUrl { get; set; } = posterUrl;

    public MovieType Type { get; set; } = type;
}

public enum MovieType
{
    Series,
    Movie,
    Episode
}