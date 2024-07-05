using System.ComponentModel.DataAnnotations;

namespace DTO.OrderDto;

public class OrderRequest
{
    [Required]
    public Guid CustomerId { get; set; }
        
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than zero.")]
    public decimal TotalPrice { get; set; }
        
    [StringLength(200)]
    public string? Address { get; set; }
        
    [StringLength(50)]
    public string? Status { get; set; }
        
    [Required]
    public DateTime Date { get; set; }
        
    [Required]
    public List<OrderDetailRequest> OrderDetails { get; set; } = new List<OrderDetailRequest>();
}

public class OrderDetailRequest
{
    [Required]
    public Guid ProductId { get; set; }
        
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
        
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }
}

