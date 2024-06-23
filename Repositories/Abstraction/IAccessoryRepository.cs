using BusinessObject.Models;
using System.Linq.Expressions;

namespace Repositories.Abstraction
{
    public interface IAccessoryRepository: IRepositoryBase<Accessory>
	{
        Task<IEnumerable<Accessory>> FindPaged(
            int skip,
            int pageSize,
            Expression<Func<Accessory, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<Accessory, object?>>[] includeProperties
        );
    }
}
