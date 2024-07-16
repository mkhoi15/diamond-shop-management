using System.Linq.Expressions;
using BusinessObject.Models;

namespace Repositories.Abstraction;

public interface IOrderDetailRepository : IRepositoryBase<OrderDetail>
{
    void Delete(OrderDetail orderDetail);
}