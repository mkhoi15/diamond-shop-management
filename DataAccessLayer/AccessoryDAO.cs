using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public class AccessoryDAO : DaoBase<Accessory>, IAccessoryDAO
	{
        private readonly DiamondShopDbContext _context;
		public AccessoryDAO(DiamondShopDbContext context) : base(context)
		{
            _context = context;
		}

        public async Task<IEnumerable<Accessory>> FindPagedAsync(Expression<Func<Accessory, bool>> predicate, int skip, int pageSize, CancellationToken cancellationToken = default, params Expression<Func<Accessory, object?>>[] includeProperties)
        {
            IQueryable<Accessory> query = _context.Accessories.Where(predicate);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Accessory> FindByIdAsync(Guid id, CancellationToken cancellationToken, params Expression<Func<Accessory, object>>[] includes)
        {
            IQueryable<Accessory> query = _context.Accessories;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Accessory>> FindAsync(Expression<Func<Accessory, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<Accessory, object>>[] includes)
        {
            IQueryable<Accessory> query = _context.Accessories.Where(predicate);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<Accessory, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Accessories.CountAsync(predicate, cancellationToken);
        }
    }
}
