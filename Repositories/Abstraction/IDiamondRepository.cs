using BusinessObject.Models;
using System.Linq.Expressions;

namespace Repositories.Abstraction
{
    public interface IDiamondRepository : IRepositoryBase<Diamond>
    {
        Task<IEnumerable<Diamond>> FindPaged(
            int skip,
            int pageSize,
            Expression<Func<Diamond, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<Diamond, object?>>[] includeProperties
        );


    }
}
