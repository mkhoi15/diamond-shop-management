using DTO;
using DTO.DiamondDto;

namespace Services.Abstraction
{
    public interface IDiamondServices
    {
        DiamondResponse CreateDiamond(DiamondRequest diamondRequest);

        DiamondResponse UpdateDiamond(DiamondResponse diamondResponse);

        Task<IEnumerable<DiamondResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<PagedResult<DiamondResponse>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<DiamondResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
