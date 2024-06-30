using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO;
using DTO.DiamondDto;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services
{
    public class DiamondServices : IDiamondServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiamondRepository _diamondRepository;
        private readonly IPaperworkRepository _paperworkRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMapper _mapper;

        public DiamondServices(IDiamondRepository diamondRepository, IPaperworkRepository paperworkRepository, IMapper mapper, IUnitOfWork unitOfWork, IMediaRepository mediaRepository)
        {
            _diamondRepository = diamondRepository;
            _paperworkRepository = paperworkRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediaRepository = mediaRepository;
        }

        public DiamondResponse CreateDiamond(DiamondRequest diamondRequest)
        {
            var diamond = _mapper.Map<Diamond>(diamondRequest);

            _diamondRepository.Add(diamond);
            /*foreach (var paperwork in diamond.PaperWorks)
            {
                _paperworkRepository.Add(paperwork);
                if (paperwork.Media != null)
                {
                    _mediaRepository.Add(paperwork.Media);
                }
            }
            if (diamond.Media != null)
            {
                _mediaRepository.Add(diamond.Media);
            }
            await _unitOfWork.SaveChangeAsync();
            */

            return _mapper.Map<DiamondResponse>(diamond);
        }
        public DiamondResponse UpdateDiamond(DiamondResponse diamondResponse)
        {
            var diamond = _mapper.Map<Diamond>(diamondResponse);

            _diamondRepository.Update(diamond);

            return _mapper.Map<DiamondResponse>(diamond);
        }

        public async Task<IEnumerable<DiamondResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var diamonds = await _diamondRepository.Find(
                diamond => diamond.IsDeleted != true,
                cancellationToken,
                diamond => diamond.Promotion
            );

            return _mapper.Map<IEnumerable<DiamondResponse>>(diamonds);
        }

        public async Task<PagedResult<DiamondResponse>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (pageNumber - 1) * pageSize;

            var diamonds = await _diamondRepository.FindPaged(
                skip,
                pageSize,
                diamond => diamond.IsDeleted != true,
                cancellationToken,
                diamond => diamond.Promotion,
                diamond => diamond.Media
            );

            var diamondResponses = _mapper.Map<IEnumerable<DiamondResponse>>(diamonds);

            var pagedResult = new PagedResult<DiamondResponse>
            {
                Items = diamondResponses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = GetAllAsync(cancellationToken).Result.Count()
            };

            return pagedResult;
        }

        public async Task<DiamondResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var diamond = await _diamondRepository.FindById(
                id,
                cancellationToken,
                diamond => diamond.Promotion,
                diamond => diamond.Media
                );
            return _mapper.Map<DiamondResponse>(diamond);
        }

    }
}
