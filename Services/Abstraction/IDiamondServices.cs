using BusinessObject.Models;
using DTO;
using DTO.DiamondDto;
using System.Linq.Expressions;

namespace Services.Abstraction
{
    public interface IDiamondServices
    {
        void CreateDiamond(DiamondRequest diamondRequest);

        void UpdateDiamond(DiamondResponse diamondResponse);

        IEnumerable<DiamondResponse> GetAllAsync(CancellationToken cancellationToken);
        Task<PagedResult<DiamondResponse>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<DiamondResponse>> GetAllByConditionAsync(Expression<Func<Diamond, bool>> predicate, CancellationToken cancellationToken);
        Task<PagedResult<DiamondResponse>> GetAllByConditionAsync(Expression<Func<Diamond, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<DiamondResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
