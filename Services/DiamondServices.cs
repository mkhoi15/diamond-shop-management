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
        private readonly IMapper _mapper;

        public DiamondServices(IDiamondRepository diamondRepository, IPaperworkRepository paperworkRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _diamondRepository = diamondRepository;
            _paperworkRepository = paperworkRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<DiamondResponse> CreateDiamondAsync(DiamondRequest diamondRequest)
        {
            var diamond = _mapper.Map<Diamond>(diamondRequest);

            _diamondRepository.Add(diamond);

            await _unitOfWork.SaveChangeAsync();

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
                diamond => diamond.Promotion
            );

            var diamondResponses = _mapper.Map<IEnumerable<DiamondResponse>>(diamonds);

            var pagedResult = new PagedResult<DiamondResponse>
            {
                Items = diamondResponses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = diamonds.Count()
            };

            return pagedResult;
        }
    }
}
