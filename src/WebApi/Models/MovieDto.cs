using Domain.Entities;

namespace WebApi.Models;

public class MovieDto
{
    public required Guid Id { get; set; }
    
    public required string ImdbId { get; set; }

    public required string Title { get; set; }

    public required string Year { get; set; }

    public required IReadOnlyCollection<string> Genres { get; set; }

    public required IReadOnlyCollection<string> Actors { get; set; }

    public required string Plot { get; set; }

    public required string Country { get; set; }

    public required Uri PosterUrl { get; set; }

    public required MovieType Type { get; set; }
}