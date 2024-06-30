using AutoMapper;
using BusinessObject.Models;
using DTO.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services
{
    public class MediaServices : IMediaServices
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public MediaServices(IMediaRepository mediaRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _mediaRepository = mediaRepository;
            _mapper = mapper;
            _env = env;
        }

        public MediaResponse Add(MediaResponse mediaResponse)
        {
            var media = _mapper.Map<Media>(mediaResponse);

            _mediaRepository.Add(media);

            return _mapper.Map<MediaResponse>(media);
        }

        public void Update(MediaResponse mediaResponse)
        {
            var media = _mapper.Map<Media>(mediaResponse);

            _mediaRepository.Update(media);
        }

        public async Task<MediaResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var media = await _mediaRepository.FindById(id, cancellationToken);

            return _mapper.Map<MediaResponse>(media);
        }

        public IFormFile? GetFileFromUrl(string relativePath)
        {
            // Xây dựng đường dẫn vật lý đầy đủ đến tệp trong wwwroot
            var fullPath = Path.Combine(_env.WebRootPath, relativePath);

            // Kiểm tra tệp tin có tồn tại hay không
            if (!File.Exists(fullPath))
            {
                return null;
            }

            // Đọc nội dung tệp tin
            var fileBytes = File.ReadAllBytes(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var fileStream = new MemoryStream(fileBytes);

            // Tạo IFormFile từ MemoryStream
            IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, "name", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream" // hoặc xác định loại MIME thích hợp
            };

            return formFile;
        }

    }
}
