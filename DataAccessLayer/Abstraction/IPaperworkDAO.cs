using BusinessObject.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstraction
{
    public interface IPaperworkDAO : IDaoBase<PaperWork>
    {
        Task<IEnumerable<PaperWork>> FindPagedAsync(
            Expression<Func<PaperWork, bool>> predicate,
            int skip,
            int pageSize,
            CancellationToken cancellationToken = default,
            params Expression<Func<PaperWork, object?>>[] includeProperties
        );
    }
}
