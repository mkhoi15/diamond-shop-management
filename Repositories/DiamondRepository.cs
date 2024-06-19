using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;
using System.Linq.Expressions;

namespace Repositories
{
    public class DiamondRepository : IDiamondRepository
    {
        private readonly IDiamondDAO _diamondDao;

        public DiamondRepository(IDiamondDAO diamondDao)
        {
            _diamondDao = diamondDao;
        }

        public void Add(Diamond entity)
        {
            _diamondDao.Add(entity);
        }

        public void AddRange(ICollection<Diamond> entities)
        {
            _diamondDao.AddRange(entities);
        }

        public async Task<IEnumerable<Diamond>> Find(Expression<Func<Diamond, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Diamond, object?>>[] includeProperties)
        {
            return await _diamondDao.Find(predicate, cancellationToken, includeProperties);
        }

        public IQueryable<Diamond> FindAll()
        {
            return _diamondDao.FindAll();
        }

        public async Task<Diamond?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Diamond, object?>>[] includeProperties)
        {
            return await _diamondDao.FindById(id, cancellationToken, includeProperties);
        }

        public async Task<IEnumerable<Diamond>> FindPaged(
            int skip,
            int pageSize,
            Expression<Func<Diamond, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<Diamond, object?>>[] includeProperties)
        {
            return await _diamondDao.FindPagedAsync(predicate, skip, pageSize, cancellationToken, includeProperties);
        }

        public void Remove(Diamond entity)
        {
            _diamondDao.Remove(entity);
        }

        public void Update(Diamond entity)
        {
            _diamondDao.Update(entity);
        }
    }
}
