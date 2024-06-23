using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Helpers;

namespace diamond_shop_management.Pages.Card;

public class Card : PageModel
{
    public List<DiamondAccessory> CartItems { get; private set; }
    
    public void OnGet()
    {
        CartItems = HttpContext.Session.GetObjectFromJson<List<DiamondAccessory>>("Cart") ?? new List<DiamondAccessory>();
    }
    
    public IActionResult OnPostClearCart()
    {
        HttpContext.Session.Remove("Cart");
        return RedirectToPage();
    }
    
    public IActionResult OnPostRemoveFromCart(Guid itemId)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<DiamondAccessory>>("Cart") ?? new List<DiamondAccessory>();
        var item = cart.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            cart.Remove(item);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
        return RedirectToPage();
    }
    
}