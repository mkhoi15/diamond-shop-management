using DTO.AccessoryDto;
using DTO;
using BusinessObject.Models;
using System.Linq.Expressions;
using DTO.DiamondDto;

namespace Services.Abstraction
{
    public interface IAccessoryServices
	{
        void CreateAccessory(AccessoryRequest request);
		IEnumerable<AccessoryResponse> GetAllAccessoriesAsync(CancellationToken cancellationToken);
        Task<IEnumerable<AccessoryResponse>> GetActiveAccessoriesAsync(Expression<Func<Accessory, bool>> predicate, CancellationToken cancellationToken);
		Task<PagedResult<AccessoryResponse>> GetAccessoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<PagedResult<AccessoryResponse>> GetAccessoryShopAsync(Expression<Func<Accessory, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<AccessoryResponse> GetAccessoryByIdAsync(Guid accessoryId, CancellationToken cancellationToken);
        void UpdateAccessory(AccessoryResponse accessory);
        Task<bool> DeleteAccessory(Guid id);
        Task AddDiamondToAccessoryAsync(Guid accessoryId, Guid diamondId);
    }
}
