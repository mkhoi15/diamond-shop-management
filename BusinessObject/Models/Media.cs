namespace BusinessObject.Models;

public class Media : Entity
{ 
    public string? Url { get; set; }
    
    public Diamond? Diamond { get; set; }
    public Accessory? Accessory { get; set; }
    public PaperWork? PaperWork { get; set; }
    
}