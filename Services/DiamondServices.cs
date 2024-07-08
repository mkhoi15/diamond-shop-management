using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO;
using DTO.DiamondDto;
using Repositories.Abstraction;
using Services.Abstraction;
using System.Linq.Expressions;

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

        public void CreateDiamond(DiamondRequest diamondRequest)
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
        }
        public void UpdateDiamond(DiamondResponse diamondResponse)
        {
            var diamond = _mapper.Map<Diamond>(diamondResponse);

            _diamondRepository.Update(diamond);
        }

        public IEnumerable<DiamondResponse> GetAllAsync(CancellationToken cancellationToken)
        {
            var diamonds = _diamondRepository.FindAll();

            return _mapper.Map<IEnumerable<DiamondResponse>>(diamonds);
        }

        public async Task<PagedResult<DiamondResponse>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (pageNumber - 1) * pageSize;

            var diamonds = await _diamondRepository.FindPaged(
                skip,
                pageSize,
                diamond => diamond.IsDeleted != null,
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
                TotalItems = GetAllAsync(cancellationToken).Count()
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

        public async Task<DiamondResponse> UpdateDiamondStatusAsync(Guid id, bool status, CancellationToken cancellationToken)
        {
            var diamond = await _diamondRepository.FindById(id, cancellationToken);

            if (diamond is null)
            {
                throw new ArgumentException("Don't find diamond");
            }
            
            diamond.IsSold = status;
            
            _diamondRepository.Update(diamond);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            
            return _mapper.Map<DiamondResponse>(diamond);
            
        }

        public async Task<IEnumerable<DiamondResponse>> GetAllByConditionAsync(Expression<Func<Diamond, bool>> predicate, CancellationToken cancellationToken)
        {
            var diamonds = await _diamondRepository.Find(
                predicate,
                cancellationToken,
                diamond => diamond.Promotion,
                diamond => diamond.Media
            );

            return _mapper.Map<IEnumerable<DiamondResponse>>(diamonds);
        }

        public async Task<PagedResult<DiamondResponse>> GetAllByConditionAsync(Expression<Func<Diamond, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (pageNumber - 1) * pageSize;

            var diamonds = await _diamondRepository.FindPaged(
                skip,
                pageSize,
                predicate,
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
                TotalItems = GetAllByConditionAsync(predicate, cancellationToken).Result.Count()
            };

            return pagedResult;
        }
    }
}
