using System.ComponentModel.DataAnnotations;
using System.Globalization;
using DTO.Revenue;
using DTO.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

public class Revenue : PageModel
{
    private readonly IRevenueServices _revenueServices;
    public List<RevenueResponse> RevenueResponses { get; set; } = new List<RevenueResponse>();
    public List<UserStatistic> UserStatistics { get; set; } = new List<UserStatistic>();
    public decimal TotalRevenue { get; set; }
    public int Year { get; set; } = DateTime.Now.Year;
    public static int CurrentYear { get; set; } = DateTime.Now.Year;
    
    public Revenue(IRevenueServices revenueServices)
    {
        _revenueServices = revenueServices;
    }
    
    public async Task OnGet(int? year = 2024)
    {
        Year = year ?? DateTime.Now.Year;
        CurrentYear = Year;
        RevenueResponses = await _revenueServices.GetRevenueByYear(year);
        TotalRevenue = RevenueResponses.Sum(revenue => revenue.TotalRevenue);
        UserStatistics = await _revenueServices.GetUserStatisticsByYear(year);
    }
    
    public async Task<IActionResult> OnPost()
    {
        RevenueResponses = await _revenueServices.GetRevenueByYear(CurrentYear);
        var stream = new MemoryStream();
        using var package = new ExcelPackage(stream);
        
        // Sheet for Revenue
        var revenueSheet = package.Workbook.Worksheets.Add($"{CurrentYear} Revenue");
        revenueSheet.Cells[1, 1].LoadFromCollection(RevenueResponses.Select(r => new {
            r.Month,
            r.TotalOrder,
            r.TotalRevenue
        }), true);

        // Sheet for Orders
        // Sheet for Orders grouped by Month
        var orderSheet = package.Workbook.Worksheets.Add($"{CurrentYear} Orders");

        int currentRow = 1;
        foreach (var revenue in RevenueResponses)
        {
            // Add Month Header
            orderSheet.Cells[currentRow, 1].Value = $"Month {revenue.Month}";
            currentRow++;

            // Add Orders for the month
            foreach (var order in revenue.Orders)
            {
                orderSheet.Cells[currentRow, 1].Value = order.Id;
                orderSheet.Cells[currentRow, 2].Value = order.UserName;
                orderSheet.Cells[currentRow, 3].Value = order.Email;
                orderSheet.Cells[currentRow, 4].Value = order.TotalPrice;
                orderSheet.Cells[currentRow, 5].Value = order.Date.ToString("yyyy-MM-dd"); // Fixing the date format
                currentRow++;
            }
            // Add Total Price for the month
            orderSheet.Cells[currentRow, 4].Value = "Total";
            orderSheet.Cells[currentRow, 5].Value = revenue.TotalRevenue;
            currentRow++;
            // Add an empty row after each month group
            currentRow++;
        }
        // Add Total Revenue for the year
        orderSheet.Cells[currentRow, 4].Value = "Total Revenue";
        orderSheet.Cells[currentRow, 5].Formula = RevenueResponses.Sum(r => r.TotalRevenue).ToString(CultureInfo.InvariantCulture);
        orderSheet.Cells.AutoFitColumns();
        
        await package.SaveAsync();
        stream.Position = 0;
        string excelName = $"Revenue_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
    }
}