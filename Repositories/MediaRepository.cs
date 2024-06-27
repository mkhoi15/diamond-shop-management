using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Repositories.Abstraction;
using System.Linq.Expressions;

namespace Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly IMediaDAO _mediaDAO;

        public MediaRepository(IMediaDAO mediaDAO)
        {
            _mediaDAO = mediaDAO;
        }

        public void Add(Media entity)
        {
            _mediaDAO.Add(entity);
        }

        public void AddRange(ICollection<Media> entities)
        {
            _mediaDAO.AddRange(entities);
        }

        public async Task<IEnumerable<Media>> Find(Expression<Func<Media, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<Media, object?>>[] includeProperties)
        {
            return await _mediaDAO.Find(predicate, cancellationToken, includeProperties);
        }

        public IQueryable<Media> FindAll()
        {
            return _mediaDAO.FindAll();
        }

        public async Task<Media?> FindById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Media, object?>>[] includeProperties)
        {
            return await _mediaDAO.FindById(id, cancellationToken, includeProperties);
        }

        public void Remove(Media entity)
        {
            _mediaDAO.Remove(entity);
        }

        public void Update(Media entity)
        {
            _mediaDAO.Update(entity);
        }
    }
}
