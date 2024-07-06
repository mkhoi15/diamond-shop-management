using BusinessObject.Models;

namespace Repositories.Abstraction
{
    public interface IDiamondAccessoryRepository : IRepositoryBase<DiamondAccessory>
	{
        Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId);
	}
}
