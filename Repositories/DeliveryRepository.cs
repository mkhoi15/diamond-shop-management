using System.Linq.Expressions;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;

namespace Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly IDeliveryDAO _deliveryDao;

    public DeliveryRepository(IDeliveryDAO deliveryDao)
    {
        _deliveryDao = deliveryDao;
    }

    public IQueryable<Delivery> FindAll()
    {
        return _deliveryDao.FindAll();
    }

    public async Task<Delivery?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Delivery, object?>>[] includeProperties)
    {
        return await _deliveryDao.FindById(id, cancellationToken, includeProperties); 
    }

    public async Task<IEnumerable<Delivery>> Find(Expression<Func<Delivery, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Delivery, object?>>[] includeProperties)
    {
        return await _deliveryDao.Find(predicate, cancellationToken, includeProperties);
    }

    public void Add(Delivery entity)
    {
        _deliveryDao.Add(entity);
    }

    public void AddRange(ICollection<Delivery> entities)
    {
        _deliveryDao.AddRange(entities);
    }

    public void Update(Delivery entity)
    {
        _deliveryDao.Update(entity);
    }

    public void Remove(Delivery entity)
    {
        _deliveryDao.Remove(entity);
    }
}