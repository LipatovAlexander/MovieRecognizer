using Domain;
using OneOf;
using OneOf.Types;

namespace Movies.Application;

public interface IMovieService
{
    Task<OneOf<Movie, NotFound>> FindMovieAsync(string title, CancellationToken cancellationToken);
}
