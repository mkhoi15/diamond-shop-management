using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Models;

public class User : IdentityUser<Guid>
{
    public string? UserName { get; set; }
    
    public ICollection<Order>? Orders { get; set; }
    public ICollection<Delivery>? Deliveries { get; set; }
}