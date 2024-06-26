using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DataAccessLayer.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class DiamondAccessoryDAO : DaoBase<DiamondAccessory>, IDiamondAccessoryDAO
	{
        private readonly DiamondShopDbContext _context;
		public DiamondAccessoryDAO(DiamondShopDbContext context) : base(context)
		{
            _context = context;
		}

        public async Task<IEnumerable<DiamondAccessory>> FindPagedAsync(Expression<Func<DiamondAccessory, bool>> predicate, int skip, int pageSize, CancellationToken cancellationToken = default, params Expression<Func<DiamondAccessory, object?>>[] includeProperties)
        {
            IQueryable<DiamondAccessory> query = _context.DiamondAccessories.Where(predicate);

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
