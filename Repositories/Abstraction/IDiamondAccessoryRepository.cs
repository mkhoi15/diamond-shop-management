using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstraction
{
	public interface IDiamondAccessoryRepository : IRepositoryBase<DiamondAccessory>
	{
		Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId);
	}
}
