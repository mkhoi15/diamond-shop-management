namespace DataAccessLayer.Abstraction;

public interface IUnitOfWork
{
    public Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
}