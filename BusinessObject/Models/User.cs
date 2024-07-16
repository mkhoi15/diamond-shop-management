using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Models;

public class User : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    
    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}