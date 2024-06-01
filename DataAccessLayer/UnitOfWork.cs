using DataAccessLayer.Abstraction;

namespace DataAccessLayer;

public class UnitOfWork : IUnitOfWork
{
    private readonly DiamondShopDbContext _context;

    public UnitOfWork(DiamondShopDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        var result = await _context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}