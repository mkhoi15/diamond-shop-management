using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccessLayer.Interceptor;

public class UpdateUserInterceptor : ISaveChangesInterceptor
{
    public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}