using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DataAccessLayer.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class DiamondAccessoryDAO : DaoBase<DiamondAccessory>, IDiamondAccessoryDAO
	{
        private readonly DiamondShopDbContext _context;
		public DiamondAccessoryDAO(DiamondShopDbContext context) : base(context)
		{
            _context = context;
		}
    }
}
