using DTO.OrderDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using Services.Helpers;

namespace diamond_shop_management.Pages.Orders;

public class Create : PageModel
{
    private readonly IOrderServices _orderServices;

    public Create(IOrderServices orderServices)
    {
        _orderServices = orderServices;
    }


    [BindProperty]
    public OrderRequest OrderRequest { get; set; }

    public DTO.Card CartItems { get; private set; }

    public void OnGet()
    {
        CartItems = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
        OrderRequest = new OrderRequest
        {
            OrderDetails = CartItems.Diamond.Select(d => new OrderDetailRequest
            {
                ProductId = d.Id,
                Quantity = 1,
                Price = d.Price
            }).ToList()
        };
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Assuming the customer ID is retrieved from the logged-in user's session or context
        var customerId = /* Your logic to get the customer ID */;
        OrderRequest.CustomerId = customerId;

        var result = await _orderServices.CreateOrderAsync(OrderRequest, cancellationToken);

        if (result)
        {
            // Clear the cart after successful order creation
            HttpContext.Session.Remove("Cart");
            return RedirectToPage("/Orders/Success");
        }

        ModelState.AddModelError("", "There was an error creating the order. Please try again.");
        return Page();
    }
    
    
}