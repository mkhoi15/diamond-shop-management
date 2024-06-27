namespace BusinessObject.Models;

public class DiamondAccessory : Entity
{
    public Guid? DiamondId { get; set; }
    public Guid? AccessoryId { get; set; }
    public Diamond? Diamond { get; set; }
    public Accessory? Accessory { get; set; }
    
    public OrderDetail? OrderDetail { get; set; }
}