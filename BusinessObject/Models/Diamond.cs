namespace BusinessObject.Models;

public class Diamond : Entity
{
    public string? Origin { get; set; }
    public string? Color { get; set; }
    public string? Cut { get; set; }
    public string? Clarity { get; set; }
    public string? Weight { get; set; }
    public decimal? Price { get; set; }
    public Guid? MediaId { get; set; }
    public bool? IsSold { get; set; }
    public Guid? PromotionId { get; set; }
    
    public Media? Media { get; set; }
    public ICollection<DiamondAccessory>? DiamondAccessories { get; set; }
    public Promotion? Promotion { get; set; }
    public ICollection<PaperWork>? PaperWorks { get; set; }
    
}