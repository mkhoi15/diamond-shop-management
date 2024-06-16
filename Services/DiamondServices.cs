using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
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
            _paperworkRepository.AddRange(diamond.PaperWorks);

            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<DiamondResponse>(diamond);
        }

        public async Task<IEnumerable<DiamondResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var diamonds = await _diamondRepository.Find(
                diamond => diamond.IsSold != true && diamond.IsDeleted != true,
                cancellationToken,
                diamond => diamond.PaperWorks,
                diamond => diamond.Media,
                diamond => diamond.Promotion,
                diamond => diamond.DiamondAccessories
            );

            return _mapper.Map<IEnumerable<DiamondResponse>>(diamonds);
        }
    }
}
