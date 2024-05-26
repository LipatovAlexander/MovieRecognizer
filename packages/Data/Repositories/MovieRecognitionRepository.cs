using Data.Models;
using Domain;
using Ydb.Sdk.Services.Table;

namespace Data.Repositories;

public interface IMovieRecognitionRepository : IRepository<MovieRecognition, Guid>
{
	Task<IReadOnlyCollection<MovieRecognition>> ListByUserIdAsync(Guid userId);

	Task<MovieRecognitionStatistics> GetStatisticsAsync();

	Task<IReadOnlyCollection<TopRecognizedTitle>> GetTopRecognizedMoviesAsync(int limit);
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

	public async Task<IReadOnlyCollection<TopRecognizedTitle>> GetTopRecognizedMoviesAsync(int limit)
	{
		return await _databaseContext.ExecuteAsync(
			async session => await session.MovieRecognitions.GetTopRecognizedMoviesAsync(limit));
	}
}