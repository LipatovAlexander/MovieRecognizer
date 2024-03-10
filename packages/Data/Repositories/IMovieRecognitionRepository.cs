using Domain;

namespace Data.Repositories;

public interface IMovieRecognitionRepository
{
    Task<MovieRecognition?> GetAsync(Guid id);
    Task SaveAsync(MovieRecognition movieRecognition);
}