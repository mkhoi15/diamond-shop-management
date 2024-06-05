using DTO.OrderDto;

namespace Services.Abstraction;

public interface IOrderServices
{
    Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
}