using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO;
using DTO.DiamondDto;
using DTO.PaperworkDto;
using Repositories;
using Repositories.Abstraction;
using Services.Abstraction;
using System.Linq.Expressions;

namespace Services
{
    public class PaperworkServices : IPaperworkServices
    {
        private readonly IPaperworkRepository _paperworkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaperworkServices(IPaperworkRepository paperworkRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _paperworkRepository = paperworkRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaperworkResponse> AddAsync(PaperworkRequest paperworkRequest)
        {
            var paperwork = _mapper.Map<PaperWork>(paperworkRequest);

            _paperworkRepository.Add(paperwork);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<PaperworkResponse>(paperwork);
        }

        public async Task<IEnumerable<PaperworkResponse>> GetAllAsync(Expression<Func<PaperWork, bool>> expression, CancellationToken cancellationToken)
        {
            var paperwork = await _paperworkRepository.Find(expression, cancellationToken, p => p.Media);

            return _mapper.Map<IEnumerable<PaperworkResponse>>(paperwork);
        }

        public async Task<PagedResult<PaperworkResponse>> GetAllAsync(Expression<Func<PaperWork, bool>> expression, int pageNumber, int pageSize, CancellationToken cancellationToken, params Expression<Func<PaperWork, object?>>[] includeProperties)
        {
            int skip = (pageNumber - 1) * pageSize;

            var paperworks = await _paperworkRepository.FindPaged(
                skip,
                pageSize,
                expression,
                cancellationToken,
                paperworks => paperworks.Media
            );

            var paperworkResponses = _mapper.Map<IEnumerable<PaperworkResponse>>(paperworks);

            var pagedResult = new PagedResult<PaperworkResponse>
            {
                Items = paperworkResponses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = GetAllAsync(expression, cancellationToken).Result.Count()
            };

            return pagedResult;
        }
    }
}
