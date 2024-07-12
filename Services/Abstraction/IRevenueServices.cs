using DTO.Revenue;

namespace Services.Abstraction;

public interface IRevenueServices
{
    Task<List<RevenueResponse>> GetRevenueByYear(int? year = null);
}