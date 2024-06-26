using DTO.DiamondDto;
using DTO;
using DTO.PaperworkDto;
using System.Linq.Expressions;
using BusinessObject.Models;

namespace Services.Abstraction
{
    public interface IPaperworkServices
    {
        Task<IEnumerable<PaperworkResponse>> GetAllAsync(Expression<Func<PaperWork, bool>> expression, CancellationToken cancellationToken);
        Task<PagedResult<PaperworkResponse>> GetAllAsync(
            Expression<Func<PaperWork, bool>> expression,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken,
            params Expression<Func<PaperWork, Object?>>[] includeProperties);
        Task<PaperworkResponse> AddAsync(PaperworkRequest paperworkRequest);
    }
}
