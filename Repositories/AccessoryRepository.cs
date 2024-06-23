using BusinessObject.Models;
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
		public void Add(Accessory entity)
		{
			throw new NotImplementedException();
		}

		public void AddRange(ICollection<Accessory> entities)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Accessory>> Find(Expression<Func<Accessory, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Accessory, object?>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public IQueryable<Accessory> FindAll()
		{
			throw new NotImplementedException();
		}

		public Task<Accessory?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Accessory, object?>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public void Remove(Accessory entity)
		{
			throw new NotImplementedException();
		}

		public void Update(Accessory entity)
		{
			throw new NotImplementedException();
		}
	}
}
