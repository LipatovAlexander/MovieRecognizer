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