using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DiamondAccessoryDto;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;

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
	}
}
