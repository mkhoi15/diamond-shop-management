namespace DTO.DeliveryDto;

public class DeliveryRequest
{
    public Guid OrderId { get; set; }
    public Guid DeliveryManId { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}