using BusinessObject.Models;
using DTO.PromotionDto;

namespace Services.Abstraction
{
    public interface IPromotionServices
    {
        Task<IEnumerable<PromotionResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<PromotionResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PromotionResponse> Add(PromotionRequest promotion);
        Task<PromotionResponse> Update(PromotionResponse promotion);
    }
}
