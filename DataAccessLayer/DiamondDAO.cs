using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DataAccessLayer.Common;

namespace DataAccessLayer
{
    public class DiamondDAO : DaoBase<Diamond>, IDiamondDAO
    {
        private readonly DiamondShopDbContext _context;

        public DiamondDAO(DiamondShopDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Diamond>> FindPagedAsync(Expression<Func<Diamond, bool>> predicate, int skip, int pageSize, CancellationToken cancellationToken = default, params Expression<Func<Diamond, object?>>[] includeProperties)
        {
            IQueryable<Diamond> query = _context.Diamonds.Where(predicate).OrderByDescending(d => d.CreatedAt);
            
            query = query.IncludeProperties(includeProperties);

            return await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
