using DTO.DeliveryDto;

namespace Services.Abstraction;

public interface IDeliveryServices
{
    Task<DeliveryResponse> CreateDeliveryAsync(DeliveryRequest deliveryRequest);
    Task<List<DeliveryResponse>> GetAllDeliveriesAsync();
    Task<DeliveryResponse> GetDeliveryByIdAsync(Guid deliveryId, CancellationToken cancellationToken);
    Task<DeliveryResponse> UpdateDeliveryAsync(DeliveryRequest deliveryRequest);
    Task<List<DeliveryResponse>> GetDeliveriesByDeliveryManId(Guid deliveryManId, CancellationToken cancellationToken);
    Task<List<DeliveryResponse>> GetDeliveriesByOrderId(Guid orderId, CancellationToken cancellationToken);
}