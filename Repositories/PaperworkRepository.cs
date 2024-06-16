using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;
using System.Linq.Expressions;

namespace Repositories
{
    public class PaperworkRepository : IPaperworkRepository
    {
        private readonly IPaperworkDAO _paperworkDao;

        public PaperworkRepository(IPaperworkDAO paperworkDao)
        {
            _paperworkDao = paperworkDao;
        }

        public void Add(PaperWork entity)
        {
            _paperworkDao.Add(entity);
        }

        public void AddRange(ICollection<PaperWork> entities)
        {
            _paperworkDao.AddRange(entities);
        }

        public async Task<IEnumerable<PaperWork>> Find(Expression<Func<PaperWork, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<PaperWork, object?>>[] includeProperties)
        {
            return await _paperworkDao.Find(predicate, cancellationToken, includeProperties);
        }

        public IQueryable<PaperWork> FindAll()
        {
            return _paperworkDao.FindAll();
        }

        public async Task<PaperWork?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<PaperWork, object?>>[] includeProperties)
        {
            return await _paperworkDao.FindById(id, cancellationToken, includeProperties);
        }

        public void Remove(PaperWork entity)
        {
            _paperworkDao.Remove(entity);
        }

        public void Update(PaperWork entity)
        {
            _paperworkDao.Update(entity);
        }
    }
}
