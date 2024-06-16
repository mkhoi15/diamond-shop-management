using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer;

public class OrderDAO : DaoBase<Order>, IOrderDAO
{
    public OrderDAO(DiamondShopDbContext context) : base(context)
    {
    }
}