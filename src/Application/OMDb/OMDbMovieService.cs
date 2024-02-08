using System.Net.Http.Json;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Movies.Application;
using OneOf.Types;

namespace Application.OMDb;

public sealed class OMDbMovieService(IOptions<OMDbSettings> settings, HttpClient httpClient) : IMovieService
{
    public async Task<OneOf.OneOf<Movie, NotFound>> FindMovieAsync(string title, CancellationToken cancellationToken)
    {
        var httpResponse = await httpClient.GetAsync($"http://omdbapi.com/?apikey={settings.Value.ApiKey}&t={title}", cancellationToken);
        httpResponse.EnsureSuccessStatusCode();

        var omdbMovie = await httpResponse.Content.ReadFromJsonAsync<OMDbMovie>(cancellationToken);

        if (omdbMovie?.Response != "True")
        {
            return new NotFound();
        }

        var genres = omdbMovie.Genre.Split(',', StringSplitOptions.TrimEntries);
        var actors = omdbMovie.Actors.Split(',', StringSplitOptions.TrimEntries);
        var posterUri = new Uri(omdbMovie.Poster);
        var type = Enum.Parse<MovieType>(omdbMovie.Type, true);

        return new Movie
        {
            Title = omdbMovie.Title,
            Year = omdbMovie.Year,
            Genres = genres,
            Actors = actors,
            Plot = omdbMovie.Plot,
            Country = omdbMovie.Country,
            PosterUri = posterUri,
            Type = type
        };
    }
}
