using System.ComponentModel.DataAnnotations;
using DTO.Revenue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

public class Revenue : PageModel
{
    private readonly IRevenueServices _revenueServices;
    public List<RevenueResponse> RevenueResponses { get; set; } = new List<RevenueResponse>();
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
    }
    
    public async Task<IActionResult> OnPost()
    {
        RevenueResponses = await _revenueServices.GetRevenueByYear(CurrentYear);
        var stream = new MemoryStream();
        using var package = new ExcelPackage(stream);
        var workSheet = package.Workbook.Worksheets.Add($"{Year} Revenue");
        workSheet.Cells.LoadFromCollection(RevenueResponses, true);
        await package.SaveAsync();
        stream.Position = 0;
        string excelName = $"Revenue_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
    }
}