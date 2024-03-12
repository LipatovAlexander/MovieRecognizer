using Domain;

namespace Data.Repositories;

public interface IVideoRepository
{
    Task<Video?> GetAsync(Guid id);
    Task SaveAsync(Video video);
}