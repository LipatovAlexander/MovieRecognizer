using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application;

public interface IApplicationDbContext
{
    DbSet<MovieRecognition> MovieRecognitions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}