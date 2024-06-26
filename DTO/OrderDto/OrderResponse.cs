using System.ComponentModel.DataAnnotations;

namespace DTO.OrderDto;

public class OrderResponse
{
    [Required]
    public Guid Id { get; set; }
        
    [Required]
    public Guid CustomerId { get; set; }
        
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than zero.")]
    public decimal TotalPrice { get; set; }
    
        
    [StringLength(50)]
    public string? Status { get; set; }
        
    
    public ICollection<OrderDetailResponse> OrderDetails { get; set; } = new List<OrderDetailResponse>();
}


public class OrderDetailResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
}