using System.Linq.Expressions;
using BusinessObject.Models;
using DataAccessLayer.Common;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Abstraction;

public class DaoBase<TEntity> : IDaoBase<TEntity>
    where TEntity : Entity
{
    private readonly DiamondShopDbContext _context;

    public DaoBase(DiamondShopDbContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> FindAll()
        => _context.Set<TEntity>();

    public async Task<TEntity?> FindById(Guid id, CancellationToken cancellationToken = default, 
        params Expression<Func<TEntity, object?>>[] includeProperties)
    {
        return await _context.Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, 
        params Expression<Func<TEntity, object?>>[] includeProperties)
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .Where(predicate)
            .IncludeProperties(includeProperties)
            .ToListAsync(cancellationToken);
    }

    public void Add(TEntity entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Set<TEntity>().Add(entity);
    }

    public void AddRange(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.Id = Guid.NewGuid();
        }
        _context.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        _context.Entry(entity)
            .State = EntityState.Modified;
    }

    public void Remove(TEntity entity)
    {
        entity.IsDeleted = true;
    }
}