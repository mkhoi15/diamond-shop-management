namespace BusinessObject.Models;

public class Order : Entity
{
    public Guid CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Address { get; set; }
    public string? Status { get; set; }
    public DateTime Date { get; set; }
    
    public string? Description { get; set; }
    
    public User? Customer { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    
    
    
}