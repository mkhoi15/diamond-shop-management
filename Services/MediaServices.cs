using AutoMapper;
using DTO.Media;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services
{
    public class MediaServices : IMediaServices
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IMapper _mapper;

        public MediaServices(IMediaRepository mediaRepository, IMapper mapper)
        {
            _mediaRepository = mediaRepository;
            _mapper = mapper;
        }

        public async Task<MediaResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var media = await _mediaRepository.FindById(id, cancellationToken);

            return _mapper.Map<MediaResponse>(media);
        }
    }
}
