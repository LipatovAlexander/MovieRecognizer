using Domain;
using Ydb.Sdk.Services.Table;

namespace Data;

public interface IMovieRecognitionRepository : IRepository<MovieRecognition, Guid>
{
    Task<IReadOnlyCollection<MovieRecognition>> ListByUserIdAsync(Guid userId);

    Task<MovieRecognitionStatistics> GetStatisticsAsync();
}

public class MovieRecognitionRepository(
    IDatabaseContext databaseContext,
    Func<IDatabaseSession, ISessionRepository<MovieRecognition, Guid>> sessionRepositoryProvider)
    : Repository<MovieRecognition, Guid>(databaseContext, sessionRepositoryProvider),
        IMovieRecognitionRepository
{
    private readonly IDatabaseContext _databaseContext = databaseContext;

    public async Task<IReadOnlyCollection<MovieRecognition>> ListByUserIdAsync(Guid userId)
    {
        return await _databaseContext.ExecuteAsync(async session =>
        {
            var (movieRecognitions, _) = await session.MovieRecognitions.ListByUserIdAsync(
                userId,
                TxControl.BeginSerializableRW().Commit());

            return movieRecognitions;
        });
    }

    public async Task<MovieRecognitionStatistics> GetStatisticsAsync()
    {
	    return await _databaseContext.ExecuteAsync(
		    async session => await session.MovieRecognitions.GetStatisticsAsync());
    }
}