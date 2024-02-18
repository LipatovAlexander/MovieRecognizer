using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Application;

public interface IApplicationDbContext
{
    DbSet<MovieRecognition> MovieRecognitions { get; }
    DbSet<Video> Videos { get; }
    DbSet<VideoFrame> VideoFrames { get; }
    DbSet<Movie> Movies { get; }
    DbSet<Job> Jobs { get; }
    DbSet<File> Files { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}