using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer
{
    public class PromotionDAO : DaoBase<Promotion>, IPromotionDAO
    {
        public PromotionDAO(DiamondShopDbContext context) : base(context)
        {
        }
    }
}
