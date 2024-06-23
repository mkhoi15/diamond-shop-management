using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public class PaperworkDAO : DaoBase<PaperWork>, IPaperworkDAO
    {
        private readonly DiamondShopDbContext _context;

        public PaperworkDAO(DiamondShopDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaperWork>> FindPagedAsync(Expression<Func<PaperWork, bool>> predicate, int skip, int pageSize, CancellationToken cancellationToken = default, params Expression<Func<PaperWork, object?>>[] includeProperties)
        {
            IQueryable<PaperWork> query = _context.PaperWorks.Where(predicate);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
