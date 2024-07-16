using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO;
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


        public async Task<List<DiamondAccessory>> IsDiamondAccessoryExist(List<Guid?> diamondId, Guid? customerId)
        {
            return await _diamondAccessoryRepository.FindAll()
                .Where(x => diamondId.Contains(x.DiamondId) && x.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddProducts(List<DiamondAccessoryRequest> productsRequest)
        {
            var listDiamondId = productsRequest.Select(p => p.DiamondId).ToList();
            var customerId = productsRequest.FirstOrDefault()?.CustomerId;

            var listDiamondAccessoryExist = await IsDiamondAccessoryExist(listDiamondId, customerId);

            List<DiamondAccessory> products = new List<DiamondAccessory>();

            foreach (var productRequest in productsRequest)
            {
                var product = _mapper.Map<DiamondAccessory>(productRequest);
                if (listDiamondAccessoryExist.Select(d => d.DiamondId).Contains(product.DiamondId))
                {
                    product.IsDeleted = false;
                }
                else
                {
                    products.Add(product);
                }
            }

            _diamondAccessoryRepository.AddRange(products);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindAll()
                .FirstOrDefaultAsync(x => x.DiamondId == diamondId && x.IsDeleted == false);

            if (diamondAccessory == null)
            {
                throw new Exception("Diamond Accessory not found");
            }

            return diamondAccessory;
        }
        public async Task CreateDiamondAccessoryAsync(DiamondAccessoryRequest request)
        {
            var diamondAccessory = _mapper.Map<DiamondAccessory>(request);
            _diamondAccessoryRepository.Add(diamondAccessory);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateDiamondAccessoryAsync(Guid id, DiamondAccessoryRequest request)
        {
            var existingDiamondAccessory = await _diamondAccessoryRepository.FindById(id);

            if (existingDiamondAccessory == null)
            {
                throw new Exception("Diamond Accessory not found.");
            }

            _mapper.Map(request, existingDiamondAccessory);

            _diamondAccessoryRepository.Update(existingDiamondAccessory);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteDiamondAccessoryAsync(Guid id)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindById(id);

            if (diamondAccessory == null)
            {
                throw new Exception("Diamond Accessory not found.");
            }

            _diamondAccessoryRepository.Remove(diamondAccessory);
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<DiamondAccessoryResponse> GetAllDiamondAccessories(CancellationToken cancellationToken)
        {
            var diamondAccessories = _diamondAccessoryRepository.FindAll();

            return _mapper.Map<IEnumerable<DiamondAccessoryResponse>>(diamondAccessories);
        }

        public async Task DeleteProduct(List<DiamondAccessoryRequest> productsRequest)
        {
            var listDiamondId = productsRequest.Select(p => p.DiamondId).ToList();
            var customerId = productsRequest.FirstOrDefault()?.CustomerId;

            var listDiamondAccessoryExist = await IsDiamondAccessoryExist(listDiamondId, customerId);

            foreach (var productRequest in productsRequest)
            {
                var product = _mapper.Map<DiamondAccessory>(productRequest);
                if (listDiamondAccessoryExist.Select(d => d.DiamondId).Contains(product.DiamondId))
                {
                    product.IsDeleted = true;
                }

            }

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<PagedResult<DiamondAccessoryResponse>> GetDiamondAccessoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (pageNumber - 1) * pageSize;

            var diamondAccessories = await _diamondAccessoryRepository.FindPaged(skip, pageSize, da => da.IsDeleted != true , cancellationToken, da => da.Diamond, da => da.Accessory);

            var diamondAccessoryResponses = _mapper.Map<IEnumerable<DiamondAccessoryResponse>>(diamondAccessories);

            var totalItems = await _diamondAccessoryRepository.FindAll().CountAsync(cancellationToken);

            var pagedResult = new PagedResult<DiamondAccessoryResponse>
            {
                Items = diamondAccessoryResponses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return pagedResult;
        }

        public async Task<DiamondAccessoryResponse> GetDiamondAccessoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindAll()
                .Include(da => da.Diamond) 
                .Include(da => da.Accessory)
                .Include(da => da.OrderDetail)
                .ThenInclude(od => od.Order.Customer)
                .Where(x => x.Id == id && x.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);

            if (diamondAccessory == null)
            {
                return null;
            }

            var response = _mapper.Map<DiamondAccessoryResponse>(diamondAccessory);
            if (diamondAccessory.Diamond != null)
            {
                response.Origin = diamondAccessory.Diamond.Origin;
                response.Color = diamondAccessory.Diamond.Color;
                response.Cut = diamondAccessory.Diamond.Cut;
                response.Clarity = diamondAccessory.Diamond.Clarity;
                response.Weight = diamondAccessory.Diamond.Weight;
            }
            response.DiamondDetails = $"{diamondAccessory.Diamond.Origin} {diamondAccessory.Diamond.Color} {diamondAccessory.Diamond.Cut}";
            response.AccessoryName = diamondAccessory.Accessory.Name;
            response.CustomerName = diamondAccessory.OrderDetail?.Order?.Customer.FullName;
            return response;
        }
    }
}
