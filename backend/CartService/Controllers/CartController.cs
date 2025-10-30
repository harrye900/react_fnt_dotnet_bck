using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private static readonly Dictionary<int, List<CartItem>> UserCarts = new();

    [HttpPost("add")]
    public IActionResult AddToCart([FromBody] AddToCartRequest request)
    {
        if (!UserCarts.ContainsKey(request.UserId))
            UserCarts[request.UserId] = new List<CartItem>();

        var existingItem = UserCarts[request.UserId].FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem != null)
            existingItem.Quantity += request.Quantity;
        else
            UserCarts[request.UserId].Add(new CartItem 
            { 
                ProductId = request.ProductId, 
                Quantity = request.Quantity,
                Price = request.Price
            });

        return Ok(new { message = "Item added to cart" });
    }

    [HttpGet("{userId}")]
    public IActionResult GetCart(int userId)
    {
        var cart = UserCarts.GetValueOrDefault(userId, new List<CartItem>());
        var total = cart.Sum(item => item.Price * item.Quantity);
        return Ok(new { items = cart, total });
    }
}

public class CartItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class AddToCartRequest
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}