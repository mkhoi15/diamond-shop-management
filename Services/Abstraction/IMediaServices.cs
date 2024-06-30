using DTO.Media;
using Microsoft.AspNetCore.Http;

namespace Services.Abstraction
{
    public interface IMediaServices
    {
        Task<MediaResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        IFormFile? GetFileFromUrl(string relativePath);

        MediaResponse Add(MediaResponse mediaResponse);

        void Update(MediaResponse mediaResponse);
    }
}
