using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO;
using DTO.AccessoryDto;
using DTO.DiamondDto;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Abstraction;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<AccessoryResponse> CreateAccessoryAsync(AccessoryRequest request)
        {
            var accessory = _mapper.Map<Accessory>(request);

            _AccessoryRepository.Add(accessory);

            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<AccessoryResponse>(accessory);
        }

        public async Task<PagedResult<AccessoryResponse>> GetAccessoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (pageNumber - 1) * pageSize;

            var accessories = await _AccessoryRepository.FindPaged(skip, pageSize, a => a.IsDeleted != true, cancellationToken, a => a.Promotion);

            var accessoryResponses = _mapper.Map<IEnumerable<AccessoryResponse>>(accessories);

            var pagedResult = new PagedResult<AccessoryResponse>
            {
                Items = accessoryResponses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = GetAllAccessoriesAsync(cancellationToken).Result.Count(),
                
            };

            return pagedResult;
        }

        public async Task<AccessoryResponse> GetAccessoryByIdAsync(Guid accessoryId, CancellationToken cancellationToken)
        {
            var accessory = await _AccessoryRepository.FindById(
                accessoryId,
                cancellationToken,
                a => a.Promotion
                );
            return _mapper.Map<AccessoryResponse>(accessory);
        }

        public async Task<IEnumerable<AccessoryResponse>> GetAllAccessoriesAsync(CancellationToken cancellationToken)
        {
            var accessories = await _AccessoryRepository.Find(
                accessory => accessory.IsDeleted != true,
                cancellationToken,
                accessory => accessory.Promotion);
            return _mapper.Map<IEnumerable<AccessoryResponse>>(accessories);
        }
    }
}
