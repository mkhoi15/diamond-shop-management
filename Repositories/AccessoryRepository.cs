using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class AccessoryRepository : IAccessoryRepository
	{
		private readonly IAccessoryDAO _accessoryDAO;
		public AccessoryRepository(IAccessoryDAO accessoryDAO)
		{
			_accessoryDAO = accessoryDAO;
		}
		public void Add(Accessory entity)
		{
			_accessoryDAO.Add(entity);
		}

		public void AddRange(ICollection<Accessory> entities)
		{
			_accessoryDAO.AddRange(entities);
		}

		public async Task<IEnumerable<Accessory>> Find(Expression<Func<Accessory, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Accessory, object?>>[] includeProperties)
		{
			return await _accessoryDAO.Find(predicate,cancellationToken,includeProperties);
		}

		public IQueryable<Accessory> FindAll()
		{
			return _accessoryDAO.FindAll();
		}

		public async Task<Accessory?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Accessory, object?>>[] includeProperties)
		{
            return await _accessoryDAO.FindById(id, cancellationToken, includeProperties);
		}

        public async Task<IEnumerable<Accessory>> FindPaged(int skip, int pageSize, Expression<Func<Accessory, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Accessory, object?>>[] includeProperties)
        {
			return await _accessoryDAO.FindPagedAsync(predicate, skip, pageSize, cancellationToken, includeProperties);
        }

        public void Remove(Accessory entity)
		{
			_accessoryDAO.Remove(entity);	
		}

		public void Update(Accessory entity)
		{
			_accessoryDAO.Update(entity);
		}
	}
}
