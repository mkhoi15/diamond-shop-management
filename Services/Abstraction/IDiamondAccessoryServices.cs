using BusinessObject.Models;
using DTO.DiamondAccessoryDto;

namespace Services.Abstraction
{
    public interface IDiamondAccessoryServices
    {
        Task AddProducts(List<DiamondAccessoryRequest> productsRequest);
        Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId);
        void CreateDiamondAccessory(DiamondAccessoryRequest request);
        void UpdateDiamondAccessory(DiamondAccessoryResponse response);
        Task<bool> DeleteDiamondAccessory(Guid id);
        IEnumerable<DiamondAccessoryResponse> GetAllDiamondAccessories(CancellationToken cancellationToken);
    }
}
