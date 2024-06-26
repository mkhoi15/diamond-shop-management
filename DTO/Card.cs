

using DTO.AccessoryDto;
using DTO.DiamondDto;

namespace DTO;

public class Card
{
    public List<DiamondResponse> Diamond { get; set; } = new List<DiamondResponse>();
    
    public List<AccessoryCard> Accessories { get; set; } = new List<AccessoryCard>();
    
}

public record AccessoryCard
{
    public AccessoryResponse Accessory { get; set; }
    
    public int Quantity { get; set; }
    
}