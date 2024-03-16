using Ydb.Sdk.Services.Table;

namespace Data;

public interface IRepository<TEntity, in TId>
{
    Task<(TEntity?, Transaction?)> TryGetAsync(TId id, TxControl txControl);

    async Task<(TEntity, Transaction?)> GetAsync(TId id, TxControl txControl)
    {
        var (entity, transaction) = await TryGetAsync(id, txControl);

        if (entity is null)
        {
            throw new InvalidOperationException("Entity not found");
        }

        return (entity, transaction);
    }

    Task<Transaction?> SaveAsync(TEntity entity, TxControl txControl);
}