using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class DiamondAccessoryDAO : DaoBase<DiamondAccessory>, IDiamondAccessoryDAO
	{
		public DiamondAccessoryDAO(DiamondShopDbContext context) : base(context)
		{
		}
	}
}
