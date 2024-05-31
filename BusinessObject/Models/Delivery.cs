namespace BusinessObject.Models;

public class Delivery : Entity
{
    public Guid OrderId { get; set; }
    public Guid DeliveryManId { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    
    public User? DeliveryMan { get; set; }
    public Order? Order { get; set; }
}