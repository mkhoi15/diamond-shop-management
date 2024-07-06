using DTO.AccessoryDto;
using DTO;

namespace Services.Abstraction
{
    public interface IAccessoryServices
	{
        void CreateAccessory(AccessoryRequest request);
		IEnumerable<AccessoryResponse> GetAllAccessoriesAsync(CancellationToken cancellationToken);
		Task<PagedResult<AccessoryResponse>> GetAccessoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
		Task<AccessoryResponse> GetAccessoryByIdAsync(Guid accessoryId, CancellationToken cancellationToken);
        void UpdateAccessory(AccessoryResponse accessory);
        Task<bool> DeleteAccessory(Guid id);
        Task AddDiamondToAccessoryAsync(Guid accessoryId, Guid diamondId);
    }
}
