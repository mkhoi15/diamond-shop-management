using DTO.Media;

namespace Services.Abstraction
{
    public interface IMediaServices
    {
        Task<MediaResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
