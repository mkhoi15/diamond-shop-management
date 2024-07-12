using BusinessObject.Models;
using DTO;
using DTO.DiamondAccessoryDto;

namespace Services.Abstraction
{
    public interface IDiamondAccessoryServices
    {
        Task AddProducts(List<DiamondAccessoryRequest> productsRequest);
        Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId);
        Task CreateDiamondAccessoryAsync(DiamondAccessoryRequest request);
        Task<PagedResult<DiamondAccessoryResponse>> GetDiamondAccessoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<DiamondAccessoryResponse> GetDiamondAccessoryByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateDiamondAccessoryAsync(Guid id, DiamondAccessoryRequest request);
        Task DeleteDiamondAccessoryAsync(Guid id);
        IEnumerable<DiamondAccessoryResponse> GetAllDiamondAccessories(CancellationToken cancellationToken);
        Task DeleteProduct(List<DiamondAccessoryRequest> productsRequest);

    }
}
