using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer
{
    public class DiamondDAO : DaoBase<Diamond>, IDiamondDAO
    {
        public DiamondDAO(DiamondShopDbContext context) : base(context)
        {
        }
    }
}
