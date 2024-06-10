using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer
{
    public class PaperworkDAO : DaoBase<PaperWork>, IPaperworkDAO
    {
        public PaperworkDAO(DiamondShopDbContext context) : base(context)
        {
        }
    }
}
