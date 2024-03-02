using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application;

public interface IApplicationDbContext
{
    DbSet<MovieRecognition> MovieRecognitions { get; }
    DbSet<Video> Videos { get; }
    DbSet<VideoFrame> VideoFrames { get; }
    DbSet<Movie> Movies { get; }
    DbSet<Job> Jobs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}