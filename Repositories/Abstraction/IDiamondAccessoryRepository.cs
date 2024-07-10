using BusinessObject.Models;
using System.Linq.Expressions;

namespace Repositories.Abstraction
{
    public interface IDiamondAccessoryRepository : IRepositoryBase<DiamondAccessory>
	{
        Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId);
        Task<IEnumerable<DiamondAccessory>> FindPaged(
            int skip,
            int pageSize,
            Expression<Func<DiamondAccessory, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<DiamondAccessory, object?>>[] includeProperties
        );
    }
}
