namespace BusinessObject.Models;

public class Promotion : Entity
{
    public string? Name { get; set; }
    public double DiscountRate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreateAt { get; set; }

    public ICollection<Diamond> Diamonds { get; set; } = new List<Diamond>();
    public ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();
}