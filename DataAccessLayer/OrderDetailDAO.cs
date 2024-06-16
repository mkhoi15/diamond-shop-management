using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer;

public class OrderDetailDAO : DaoBase<OrderDetail>, IOrderDetailDAO
{
    public OrderDetailDAO(DiamondShopDbContext context) : base(context)
    {
    }
}