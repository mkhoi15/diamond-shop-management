namespace BusinessObject.Models;

public class Accessory : Entity
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public Guid? MediaId { get; set; }
    public Guid? PromotionId { get; set; }
    
    public Media? Media { get; set; }
    public ICollection<DiamondAccessory>? DiamondAccessories { get; set; }
    public Promotion? Promotion { get; set; }
    
}