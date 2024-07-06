using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DTO.DiamondAccessoryDto;
using DTO.DiamondAccessoryDto;

namespace Services.Abstraction
{
	public interface IDiamondAccessoryServices
	{
		Task AddProducts(List<DiamondAccessoryRequest> productsRequest);
		
		Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId);
	}
    public interface IDiamondAccessoryServices
    {
        Task<DiamondAccessoryResponse> CreateDiamondAccessory(DiamondAccessoryRequest request);
        Task<DiamondAccessoryResponse> UpdateDiamondAccessory(Guid id, DiamondAccessoryRequest request);
        Task<bool> DeleteDiamondAccessory(Guid id);
        Task<DiamondAccessoryResponse> GetDiamondAccessoryById(Guid id);
        Task<IEnumerable<DiamondAccessoryResponse>> GetAllDiamondAccessories();
    }
}
