using Ydb.Sdk.Services.Table;

namespace Data;

public interface IRepository<TEntity, in TId>
{
    Task<TEntity?> TryGetAsync(TId id);

    async Task<TEntity> GetAsync(TId id)
    {
        return await TryGetAsync(id)
               ?? throw new InvalidOperationException($"{typeof(TEntity).Name} not found by id {id}");
    }

    Task SaveAsync(TEntity entity);
}

public class Repository<TEntity, TId>(
    IDatabaseContext databaseContext,
    Func<IDatabaseSession, ISessionRepository<TEntity, TId>> sessionRepositoryProvider) : IRepository<TEntity, TId>
{
    private readonly IDatabaseContext _databaseContext = databaseContext;

    private readonly Func<IDatabaseSession, ISessionRepository<TEntity, TId>> _sessionRepositoryProvider =
        sessionRepositoryProvider;

    public async Task<TEntity?> TryGetAsync(TId id)
    {
        return await _databaseContext.ExecuteAsync(async session =>
        {
            var sessionRepository = _sessionRepositoryProvider(session);
            var (entity, _) = await sessionRepository.TryGetAsync(id, TxControl.BeginSerializableRW().Commit());
            return entity;
        });
    }

    public async Task SaveAsync(TEntity entity)
    {
        await _databaseContext.ExecuteAsync(async session =>
        {
            var sessionRepository = _sessionRepositoryProvider(session);
            await sessionRepository.SaveAsync(entity, TxControl.BeginSerializableRW().Commit());
        });
    }
}