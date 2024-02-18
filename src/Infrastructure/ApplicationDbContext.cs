using System.Text.Json;
using Application;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using File = Domain.Entities.File;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<MovieRecognition> MovieRecognitions => Set<MovieRecognition>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<VideoFrame> VideoFrames => Set<VideoFrame>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<File> Files => Set<File>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entityType.Name).ToTable(entityType.ClrType.Name);
        }

        modelBuilder.Entity<Movie>()
            .Property(m => m.Genres)
            .HasConversion<string>(
                collection => JsonSerializer.Serialize(collection, JsonSerializerOptions.Default),
                @string => JsonSerializer.Deserialize<IReadOnlyCollection<string>>(@string, JsonSerializerOptions.Default)!,
                ValueComparer.CreateDefault<IReadOnlyCollection<string>>(true));

        modelBuilder.Entity<Movie>()
            .Property(m => m.Actors)
            .HasConversion<string>(
                collection => JsonSerializer.Serialize(collection, JsonSerializerOptions.Default),
                @string => JsonSerializer.Deserialize<IReadOnlyCollection<string>>(@string, JsonSerializerOptions.Default)!,
                ValueComparer.CreateDefault<IReadOnlyCollection<string>>(true));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }
}