using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer;

public class OrderDetailDAO : DaoBase<OrderDetail>, IOrderDetailDAO
{
    private readonly DiamondShopDbContext _context;
    public OrderDetailDAO(DiamondShopDbContext context) : base(context)
    {
        _context = context;
    }

    public void Delete(OrderDetail orderDetail)
    {
        _context.OrderDetails.Remove(orderDetail);
    }
}