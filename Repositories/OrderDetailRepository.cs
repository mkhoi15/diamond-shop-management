using System.Linq.Expressions;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;

namespace Repositories;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly IOrderDetailDAO _orderDetailDao;

    public OrderDetailRepository(IOrderDetailDAO orderDetailDao)
    {
        _orderDetailDao = orderDetailDao;
    }

    public IQueryable<OrderDetail> FindAll()
    {
        return _orderDetailDao.FindAll();
    }

    public async Task<OrderDetail?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<OrderDetail, object?>>[] includeProperties)
    {
        return await _orderDetailDao.FindById(id, cancellationToken, includeProperties);
    }

    public async Task<IEnumerable<OrderDetail>> Find(Expression<Func<OrderDetail, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<OrderDetail, object?>>[] includeProperties)
    {
        return await _orderDetailDao.Find(predicate, cancellationToken, includeProperties);
    }

    public void Add(OrderDetail entity)
    {
        _orderDetailDao.Add(entity);
    }

    public void AddRange(ICollection<OrderDetail> entities)
    {
        _orderDetailDao.AddRange(entities);
    }

    public void Update(OrderDetail entity)
    {
        _orderDetailDao.Update(entity);
    }

    public void Remove(OrderDetail entity)
    {
        _orderDetailDao.Remove(entity);
    }
}