using BusinessObject.Models;
using DTO.DiamondDto;
using DTO.OrderDto;

namespace Services.Abstraction;

public interface IOrderServices
{
    Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
    Task<List<OrderResponse>> GetAllOrdersAsync();
    Task<OrderResponse> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task<OrderResponse> UpdateOrderAsync(Order order);
    Task<List<DiamondResponse>> GetDiamondsByOrderId(Guid orderId, CancellationToken cancellationToken);
    Task<List<OrderResponse>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken);
}