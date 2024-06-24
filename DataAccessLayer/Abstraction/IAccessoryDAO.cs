using BusinessObject.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstraction
{
    public interface IAccessoryDAO : IDaoBase<Accessory>
	{
        Task<IEnumerable<Accessory>> FindPagedAsync(
            Expression<Func<Accessory, bool>> predicate,
            int skip,
            int pageSize,
            CancellationToken cancellationToken = default,
            params Expression<Func<Accessory, object?>>[] includeProperties
        );
    }
}
