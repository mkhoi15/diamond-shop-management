namespace DTO.Revenue;

public class RevenueResponse
{
    public string? Month { get; set; }
    public int TotalOrder { get; set; }
    public decimal TotalRevenue { get; set; }
}