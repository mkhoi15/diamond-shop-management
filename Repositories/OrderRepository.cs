using System.Linq.Expressions;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;

namespace Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IOrderDAO _orderDao;

    public OrderRepository(IOrderDAO orderDao)
    {
        _orderDao = orderDao;
    }

    public IQueryable<Order> FindAll()
    {
        return _orderDao.FindAll();
    }

    public async Task<Order?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Order, object?>>[] includeProperties)
    {
        return await _orderDao.FindById(id, cancellationToken, includeProperties);
    }

    public async Task<IEnumerable<Order>> Find(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Order, object?>>[] includeProperties)
    {
        return await _orderDao.Find(predicate, cancellationToken, includeProperties);
    }

    public void Add(Order entity)
    {
        _orderDao.Add(entity);
    }

    public void AddRange(ICollection<Order> entities)
    {
        _orderDao.AddRange(entities);
    }

    public void Update(Order entity)
    {
        _orderDao.Update(entity);
    }

    public void Remove(Order entity)
    {
        _orderDao.Remove(entity);
    }
}