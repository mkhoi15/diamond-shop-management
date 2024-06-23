using BusinessObject.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstraction
{
    public interface IDiamondDAO : IDaoBase<Diamond>
    {
        Task<IEnumerable<Diamond>> FindPagedAsync(
            Expression<Func<Diamond, bool>> predicate,
            int skip,
            int pageSize,
            CancellationToken cancellationToken = default,
            params Expression<Func<Diamond, object?>>[] includeProperties
        );
    }
}
