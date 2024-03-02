using Domain.Entities;
using WebApi.Models;

namespace WebApi.Mappers;

public static class MovieMapper
{
    public static MovieDto ToDto(this Movie movie)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Actors = movie.Actors,
            Country = movie.Country,
            Genres = movie.Genres,
            Plot = movie.Plot,
            Title = movie.Title,
            Type = movie.Type,
            Year = movie.Year,
            ExternalId = movie.ExternalId,
            PosterUrl = movie.PosterUrl
        };
    }
}