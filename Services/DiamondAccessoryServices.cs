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
        private readonly IDiamondAccessoryRepository _diamondAccessoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DiamondAccessoryServices(IDiamondAccessoryRepository diamondAccessoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _diamondAccessoryRepository = diamondAccessoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddProducts(List<DiamondAccessoryRequest> productsRequest)
        {
            List<DiamondAccessory> products = new List<DiamondAccessory>();

            foreach (var productRequest in productsRequest)
            {
                var product = _mapper.Map<DiamondAccessory>(productRequest);
                products.Add(product);
            }

            _diamondAccessoryRepository.AddRange(products);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindAll()
                .FirstOrDefaultAsync(x => x.DiamondId == diamondId);

            if (diamondAccessory == null)
            {
                throw new Exception("Diamond Accessory not found");
            }

            return diamondAccessory;
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
