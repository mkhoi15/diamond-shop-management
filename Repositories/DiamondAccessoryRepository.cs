using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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
			_diamondAccessoryDAO.AddRange(entities);
		}

		public async Task<IEnumerable<DiamondAccessory>> Find(Expression<Func<DiamondAccessory, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<DiamondAccessory, object?>>[] includeProperties)
		{
			return await _diamondAccessoryDAO.Find(predicate, cancellationToken, includeProperties);
		}

		public IQueryable<DiamondAccessory> FindAll()
		{
			 return _diamondAccessoryDAO.FindAll();
		}

		public async Task<DiamondAccessory?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<DiamondAccessory, object?>>[] includeProperties)
		{
			return await _diamondAccessoryDAO.FindById(id, cancellationToken, includeProperties);
		}

		public void Remove(DiamondAccessory entity)
		{
			_diamondAccessoryDAO.Remove(entity);
		}

		public async Task<DiamondAccessory> GetProductByDiamondId(Guid diamondId)
		{
			var diamondAccessory = await _diamondAccessoryDAO.FindAll()
				.FirstOrDefaultAsync(x => x.DiamondId == diamondId);

			if (diamondAccessory == null)
			{
				throw new Exception("Diamond Accessory not found");
			}
			
			return diamondAccessory;
		}


		public void Update(DiamondAccessory entity)
		{
			_diamondAccessoryDAO.Update(entity);
		}
    }
}
