using System.Linq.Expressions;

namespace DataAccessLayer.Abstraction;

public interface IDaoBase<TEntity>
{
    public IQueryable<TEntity> FindAll();

    public Task<TEntity?> FindById(Guid id, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeProperties);
    
    public Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeProperties);

    public void Add(TEntity entity);
    
    public void AddRange(ICollection<TEntity> entities);
    
    public void Update(TEntity entity);

    public void Remove(TEntity entity);
}