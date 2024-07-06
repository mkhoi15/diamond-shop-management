using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DiamondAccessoryDto;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services
{
    public class DiamondAccessoryServices : IDiamondAccessoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiamondAccessoryRepository _diamondAccessoryRepository;
        private readonly IMapper _mapper;
        public DiamondAccessoryServices(IUnitOfWork unitOfWork, IDiamondAccessoryRepository diamondAccessoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _diamondAccessoryRepository = diamondAccessoryRepository;
            _mapper = mapper;
        }
        public async Task<DiamondAccessoryResponse> CreateDiamondAccessory(DiamondAccessoryRequest request)
        {
            var diamondAccessory = _mapper.Map<DiamondAccessory>(request);
            _diamondAccessoryRepository.Add(diamondAccessory);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<DiamondAccessoryResponse>(diamondAccessory);
        }

        public async Task<DiamondAccessoryResponse> UpdateDiamondAccessory(Guid id, DiamondAccessoryRequest request)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindById(id);
            if (diamondAccessory == null)
            {
                throw new KeyNotFoundException("DiamondAccessory not found");
            }

            _mapper.Map(request, diamondAccessory);
            _diamondAccessoryRepository.Update(diamondAccessory);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<DiamondAccessoryResponse>(diamondAccessory);
        }

        public async Task<bool> DeleteDiamondAccessory(Guid id)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindById(id);
            if (diamondAccessory == null)
            {
                return false;
            }

            _diamondAccessoryRepository.Remove(diamondAccessory);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<DiamondAccessoryResponse> GetDiamondAccessoryById(Guid id)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindById(id);
            if (diamondAccessory == null)
            {
                throw new KeyNotFoundException("DiamondAccessory not found");
            }

            return _mapper.Map<DiamondAccessoryResponse>(diamondAccessory);
        }

        public async Task<IEnumerable<DiamondAccessoryResponse>> GetAllDiamondAccessories()
        {
            var diamondAccessories = await _diamondAccessoryRepository.FindAll().ToListAsync();
            return _mapper.Map<IEnumerable<DiamondAccessoryResponse>>(diamondAccessories);
        }
    }
}
