namespace DTO.DeliveryDto;

public class DeliveryResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid DeliveryManId { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public string? DeliveryManFullName { get; set; }
    public DateTime CreatedAt { get; set; }
    
}