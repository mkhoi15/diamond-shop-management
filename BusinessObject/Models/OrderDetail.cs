namespace BusinessObject.Models;

public class OrderDetail : Entity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public Order? Order { get; set; }
    public DiamondAccessory? Product { get; set; }
    
}