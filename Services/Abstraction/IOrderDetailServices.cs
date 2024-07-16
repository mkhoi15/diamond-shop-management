using BusinessObject.Models;
using DTO.OrderDto;

namespace Services.Abstraction;

public interface IOrderDetailServices
{
    Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task<bool> DeleteOrderDetailAsync(Guid orderId, CancellationToken cancellationToken);

    Task<OrderDetail?> GetOrderDetailByProductId(Guid productId, CancellationToken cancellationToken);
}