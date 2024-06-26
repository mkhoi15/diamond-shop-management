using BusinessObject.Models;
using System.Linq.Expressions;

namespace Repositories.Abstraction
{
    public interface IPaperworkRepository : IRepositoryBase<PaperWork>
    {
        Task<IEnumerable<PaperWork>> FindPaged(
                       int skip,
                       int pageSize,
                       Expression<Func<PaperWork, bool>> predicate,
                       CancellationToken cancellationToken = default,
                       params Expression<Func<PaperWork, object?>>[] includeProperties
                   );
    }
}
