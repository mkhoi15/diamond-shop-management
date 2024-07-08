using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.PromotionDto;
using Repositories.Abstraction;
using Services.Abstraction;
using System.Linq.Expressions;

namespace Services
{
    public class PromotionServices : IPromotionServices
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromotionServices(IPromotionRepository promotionRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _promotionRepository = promotionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PromotionResponse> Add(PromotionRequest promotion)
        {
            var addPromotion = _mapper.Map<Promotion>(promotion);

            _promotionRepository.Add(addPromotion);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<PromotionResponse>(addPromotion);
        }

        public async Task<IEnumerable<PromotionResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var promotions = await _promotionRepository.Find(p => p.IsActive != false && p.IsDeleted != true, cancellationToken);

            return _mapper.Map<IEnumerable<PromotionResponse>>(promotions);
        }

        public async Task<PromotionResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var promotion = await _promotionRepository.FindById(id);

            return _mapper.Map<PromotionResponse>(promotion);
        }

        public async Task<IEnumerable<PromotionResponse>> GetPromotionsByCondition(Expression<Func<Promotion, bool>> expression, CancellationToken cancellationToken, params Expression<Func<Promotion, object?>>[] includeProperties)
        {
            var promotion = await _promotionRepository.Find(expression, cancellationToken, includeProperties);

            return _mapper.Map<IEnumerable<PromotionResponse>>(promotion);
        }

        public async Task<PromotionResponse> Update(PromotionResponse promotion)
        {
            var updatePromotion = _mapper.Map<Promotion>(promotion);

            _promotionRepository.Update(updatePromotion);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<PromotionResponse>(updatePromotion);
        }
    }
}
