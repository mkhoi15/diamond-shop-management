using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;
using System.Linq.Expressions;

namespace Repositories
{
    internal class DiamondAccessoryRepository : IDiamondAccessoryRepository
	{
		private readonly IDiamondAccessoryDAO _diamondAccessoryDAO;

		public DiamondAccessoryRepository(IDiamondAccessoryDAO diamondAccessoryDAO)
		{
			_diamondAccessoryDAO = diamondAccessoryDAO;
		}
		public void Add(DiamondAccessory entity)
		{
			_diamondAccessoryDAO.Add(entity);
		}

		public void AddRange(ICollection<DiamondAccessory> entities)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<DiamondAccessory>> Find(Expression<Func<DiamondAccessory, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<DiamondAccessory, object?>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public IQueryable<DiamondAccessory> FindAll()
		{
			throw new NotImplementedException();
		}

		public Task<DiamondAccessory?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<DiamondAccessory, object?>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public void Remove(DiamondAccessory entity)
		{
			throw new NotImplementedException();
		}

		public void Update(DiamondAccessory entity)
		{
			throw new NotImplementedException();
		}
	}
}
