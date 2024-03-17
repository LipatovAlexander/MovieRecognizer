using Domain;

namespace Data;

public interface IDatabaseContext
{
    IRepository<MovieRecognition, Guid> MovieRecognitions { get; }
    IRepository<Video, Guid> Videos { get; }
}

public class DatabaseContext(
    IRepository<MovieRecognition, Guid> movieRecognitions,
    IRepository<Video, Guid> videos)
    : IDatabaseContext
{
    public IRepository<MovieRecognition, Guid> MovieRecognitions { get; } = movieRecognitions;
    public IRepository<Video, Guid> Videos { get; } = videos;
}