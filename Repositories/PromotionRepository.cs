using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;
using System.Linq.Expressions;

namespace Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly IPromotionDAO _promotionDAO;

        public PromotionRepository(IPromotionDAO promotionDAO)
        {
            _promotionDAO = promotionDAO;
        }

        public void Add(Promotion entity)
        {
            _promotionDAO.Add(entity);
        }

        public void AddRange(ICollection<Promotion> entities)
        {
            _promotionDAO.AddRange(entities);
        }

        public async Task<IEnumerable<Promotion>> Find(Expression<Func<Promotion, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Promotion, object?>>[] includeProperties)
        {
            return await _promotionDAO.Find(predicate, cancellationToken, includeProperties);
        }

        public IQueryable<Promotion> FindAll()
        {
            return _promotionDAO.FindAll();
        }

        public async Task<Promotion?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Promotion, object?>>[] includeProperties)
        {
            return await _promotionDAO.FindById(id, cancellationToken, includeProperties);
        }

        public void Remove(Promotion entity)
        {
            _promotionDAO.Remove(entity);
        }

        public void Update(Promotion entity)
        {
            _promotionDAO.Update(entity);
        }
    }
}
