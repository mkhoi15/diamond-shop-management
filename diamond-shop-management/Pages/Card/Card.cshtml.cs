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
    
    public decimal Total { get; private set; }


    public List<DiamondResponse> Diamonds { get; set; } = new List<DiamondResponse>();
    
    public void OnGet()
    {
        CartItems = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
        this.TotalPrice();
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
    
    public IActionResult OnPostRemoveFromCart(Guid itemId)
    {
        var cart = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
        var item = cart.Diamond.FirstOrDefault(x => x.Id == itemId);
        if (item != null)
        {
            cart.Diamond.Remove(item);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
        return RedirectToPage();
    }


    public void TotalPrice()
    {
        decimal total = 0;
        if (this.CartItems.Diamond.Count > 0)
        {
            foreach (var diamond in this.CartItems.Diamond)
            {
                if (diamond.Price != null)
                {
                    total = total + decimal.Parse(diamond.Price.ToString()!);
                }
            }
        }

        this.Total = total;

    } 
    
    
}