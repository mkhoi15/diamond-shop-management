using BusinessObject.Models;
using DTO.PromotionDto;
using System.Linq.Expressions;

namespace Services.Abstraction
{
    public interface IPromotionServices
    {
        Task<IEnumerable<PromotionResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<PromotionResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<PromotionResponse>> GetPromotionsByCondition(Expression<Func<Promotion, bool>> expression, CancellationToken cancellationToken, params Expression<Func<Promotion, object?>>[] includeProperties);
        Task<PromotionResponse> Add(PromotionRequest promotion);
        Task<PromotionResponse> Update(PromotionResponse promotion);
    }
}
