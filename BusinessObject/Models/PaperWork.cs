namespace BusinessObject.Models;

public class PaperWork : Entity
{
    public Guid DiamondId { get; set; }
    public string? Type { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? Status { get; set; }
    public Guid? MediaId { get; set; }
    
    public Media? Media { get; set; }
    public Diamond? Diamond { get; set; }
    
}