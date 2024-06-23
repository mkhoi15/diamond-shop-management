using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class AccessoryDAO : DaoBase<Accessory>, IAccessoryDAO
	{
		public AccessoryDAO(DiamondShopDbContext context) : base(context)
		{
		}
	}
}
