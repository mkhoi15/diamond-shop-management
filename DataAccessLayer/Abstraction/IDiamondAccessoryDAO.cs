using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
	public interface IDiamondAccessoryDAO : IDaoBase<DiamondAccessory>
	{
        Task<IEnumerable<DiamondAccessory>> FindPagedAsync(
            Expression<Func<DiamondAccessory, bool>> predicate,
            int skip,
            int pageSize,
            CancellationToken cancellationToken = default,
            params Expression<Func<DiamondAccessory, object?>>[] includeProperties
        );
    }
}
