using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DTO.DiamondAccessoryDto;

namespace Services.Abstraction
{
	public interface IDiamondAccessoryServices
	{
		Task AddProducts(List<DiamondAccessoryRequest> productsRequest);
		
		Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId);
	}
}
