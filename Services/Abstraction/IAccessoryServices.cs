using DTO.AccessoryDto;
using DTO;

namespace Services.Abstraction
{
    public interface IAccessoryServices
	{
		Task<AccessoryResponse> CreateAccessoryAsync(AccessoryRequest request);
		Task<IEnumerable<AccessoryResponse>> GetAllAccessoriesAsync(CancellationToken cancellationToken);
		Task<PagedResult<AccessoryResponse>> GetAccessoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
		Task<AccessoryResponse> GetAccessoryByIdAsync(Guid accessoryId, CancellationToken cancellationToken);
        Task AddDiamondToAccessoryAsync(Guid accessoryId, Guid diamondId);
    }
}
