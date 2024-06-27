using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer
{
    public class MediaDAO : DaoBase<Media>, IMediaDAO
    {
        public MediaDAO(DiamondShopDbContext context) : base(context)
        {
        }
    }
}
