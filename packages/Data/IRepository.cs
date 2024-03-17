namespace Data;

public interface IRepository<TEntity, in TId>
{
    Task<TEntity?> GetAsync(TId id);

    Task SaveAsync(TEntity entity);
}