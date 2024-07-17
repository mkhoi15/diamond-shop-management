using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO;
using DTO.AccessoryDto;
using DTO.DiamondAccessoryDto;
using DTO.DiamondDto;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Abstraction;
using Services.Abstraction;
using System.Linq.Expressions;

namespace Services
{
    public class AccessoryServices : IAccessoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessoryRepository _AccessoryRepository;
        private readonly IDiamondAccessoryRepository _DiamondAccessoryRepository;
        private readonly IMapper _mapper;

        public AccessoryServices(IUnitOfWork unitOfWork, IAccessoryRepository accessoryRepository, IDiamondAccessoryRepository diamondAccessoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _AccessoryRepository = accessoryRepository;
            _DiamondAccessoryRepository = diamondAccessoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> AccessoryNameExistsAsync(string name, CancellationToken cancellationToken)
        {
            var accessories = await _AccessoryRepository.Find(a => a.Name == name, cancellationToken);
            return accessories.Any();
        }

        public async Task AddDiamondToAccessoryAsync(Guid accessoryId, Guid diamondId)
        {
            var diamondAccessory = new DiamondAccessory
            {
                AccessoryId = accessoryId,
                DiamondId = diamondId
            };

            _DiamondAccessoryRepository.Add(diamondAccessory);
            await _unitOfWork.SaveChangeAsync();
        }

        public void CreateAccessory(AccessoryRequest request)
        {
            var accessory = _mapper.Map<Accessory>(request);

            _AccessoryRepository.Add(accessory);
        }

        public async Task<bool> DeleteAccessory(Guid id)
        {
            var accessory = await _AccessoryRepository.FindById(id);

            if (accessory == null)
            {
                return false;
            }

            _AccessoryRepository.Remove(accessory);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<PagedResult<AccessoryResponse>> GetAccessoriesAsync(Expression<Func<Accessory, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (pageNumber - 1) * pageSize;

            var accessories = await _AccessoryRepository.FindPaged(skip, pageSize, predicate, cancellationToken, a => a.Promotion);

            var totalItems = await _AccessoryRepository.FindAll()
                                                       .Where(predicate)
                                                       .CountAsync(cancellationToken);

            var accessoryResponses = _mapper.Map<IEnumerable<AccessoryResponse>>(accessories);

            var pagedResult = new PagedResult<AccessoryResponse>
            {
                Items = accessoryResponses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return pagedResult;
        }

        public async Task<AccessoryResponse> GetAccessoryByIdAsync(Guid accessoryId, CancellationToken cancellationToken)
        {
            var accessory = await _AccessoryRepository.FindById(
                accessoryId,
                cancellationToken,
                a => a.Promotion,
                a => a.Media
                );

            return _mapper.Map<AccessoryResponse>(accessory);
        }

        public async Task<PagedResult<AccessoryResponse>> GetAccessoryShopAsync(Expression<Func<Accessory, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (pageNumber - 1) * pageSize;

            var accessories = await _AccessoryRepository.FindPaged(
                skip,
                pageSize,
                predicate,
                cancellationToken,
                a => a.Promotion,
                a => a.Media
            );

            var accessoryResponse = _mapper.Map<IEnumerable<AccessoryResponse>>(accessories);

            var totalItems = await _AccessoryRepository.FindAll()
                                                       .Where(predicate)
                                                       .CountAsync(cancellationToken);

            var pagedResult = new PagedResult<AccessoryResponse>
            {
                Items = accessoryResponse,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
            };

            return pagedResult;
        }

        public async Task<IEnumerable<AccessoryResponse>> GetActiveAccessoriesAsync(Expression<Func<Accessory, bool>> predicate, CancellationToken cancellationToken)
        {
            var accessories = await _AccessoryRepository.Find(
                predicate,
                cancellationToken);

            return _mapper.Map<IEnumerable<AccessoryResponse>>(accessories);
        }

        public IEnumerable<AccessoryResponse> GetAllAccessoriesAsync(CancellationToken cancellationToken)
        {
            var accessories = _AccessoryRepository.FindAll();

            return _mapper.Map<IEnumerable<AccessoryResponse>>(accessories);
        }

        public void UpdateAccessory(AccessoryResponse response)
        {
            var entity = _mapper.Map<Accessory>(response);

            _AccessoryRepository.Update(entity);
        }
    }
}
