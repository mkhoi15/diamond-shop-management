using BusinessObject.Models;
using DTO.DiamondDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Helpers;

namespace diamond_shop_management.Pages.Card;

public class Card : PageModel
{
    public DTO.Card CartItems { get; private set; } 
    //public List<Card> CartItems { get; private set; }


    public List<DiamondResponse> Diamonds { get; set; } = new List<DiamondResponse>();
    
    public void OnGet()
    {
        CartItems = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
        
    }
    
    public IActionResult OnPostClearCart()
    {
        HttpContext.Session.Remove("Cart");
        return RedirectToPage();
    }
    
    // public IActionResult OnPostRemoveFromCart(Guid itemId)
    // {
    //     var cart = HttpContext.Session.GetObjectFromJson<Card>("Cart") ?? new Card();
    //     Card item = cart.CartItems.FirstOrDefault(x => x.Id == itemId);
    //     if (item != null)
    //     {
    //         cart.Remove(item);
    //         HttpContext.Session.SetObjectAsJson("Cart", cart);
    //     }
    //     return RedirectToPage();
    // }
    
}