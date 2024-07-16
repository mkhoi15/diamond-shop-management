using DTO.Revenue;
using DTO.UserDto;

namespace Services.Abstraction;

public interface IRevenueServices
{
    Task<List<RevenueResponse>> GetRevenueByYear(int? year = null);
    
    Task<List<UserStatistic>> GetUserStatisticsByYear(int? year = null);
}