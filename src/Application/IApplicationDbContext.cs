using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application;

public interface IApplicationDbContext
{
    DbSet<RecognitionRequest> RecognitionRequests { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}