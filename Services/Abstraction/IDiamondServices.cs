using DTO.DiamondDto;

namespace Services.Abstraction
{
    public interface IDiamondServices
    {
        Task<DiamondResponse> CreateDiamondAsync(DiamondRequest diamondRequest);

        Task<IEnumerable<DiamondResponse>> GetAllAsync(CancellationToken cancellationToken);
    }
}
