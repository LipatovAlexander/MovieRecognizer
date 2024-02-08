using OneOf;
using OneOf.Types;

namespace Application.Movies;

public interface IMovieService
{
    Task<OneOf<Movie, NotFound>> FindMovieAsync(string title, CancellationToken cancellationToken);
}
