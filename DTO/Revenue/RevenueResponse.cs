namespace DTO.Revenue;

public class RevenueResponse
{
    public string? Month { get; set; }
    public int TotalOrder { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal RevenueGrowth { get; set; }
    public List<OrderExcel> Orders { get; set; } = new();
}

public record OrderExcel
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime Date { get; set; }
}