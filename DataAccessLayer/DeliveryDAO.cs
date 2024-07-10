using BusinessObject.Models;
using DataAccessLayer.Abstraction;

namespace DataAccessLayer;

public class DeliveryDAO : DaoBase<Delivery>, IDeliveryDAO
{
    public DeliveryDAO(DiamondShopDbContext context) : base(context)
    {
    }
}