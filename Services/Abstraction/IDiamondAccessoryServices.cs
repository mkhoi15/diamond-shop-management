using DTO.DiamondAccessoryDto;

namespace Services.Abstraction
{
    public interface IDiamondAccessoryServices
    {
        Task<DiamondAccessoryResponse> CreateDiamondAccessory(DiamondAccessoryRequest request);
        Task<DiamondAccessoryResponse> UpdateDiamondAccessory(Guid id, DiamondAccessoryRequest request);
        Task<bool> DeleteDiamondAccessory(Guid id);
        Task<DiamondAccessoryResponse> GetDiamondAccessoryById(Guid id);
        Task<IEnumerable<DiamondAccessoryResponse>> GetAllDiamondAccessories();
    }
}
