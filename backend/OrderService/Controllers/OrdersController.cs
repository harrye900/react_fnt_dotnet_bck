using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IMongoCollection<Order> _orders;

    public OrdersController(HttpClient httpClient, IMongoDatabase database)
    {
        _httpClient = httpClient;
        _orders = database.GetCollection<Order>("orders");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        // Call Payment Service
        var json = JsonSerializer.Serialize(request.PaymentDetails);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var paymentResponse = await _httpClient.PostAsync(
            "http://localhost:5004/api/payment/process", 
            content);

        if (!paymentResponse.IsSuccessStatusCode)
            return BadRequest(new { message = "Payment failed" });

        var order = new Order
        {
            UserId = request.UserId,
            Items = request.Items,
            Total = request.Total,
            Status = "Confirmed",
            CreatedAt = DateTime.UtcNow
        };

        await _orders.InsertOneAsync(order);
        return Ok(new { message = "Order created successfully", orderId = order.Id });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        var userOrders = await _orders.Find(o => o.UserId == userId).ToListAsync();
        return Ok(userOrders);
    }
}

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal Total { get; set; }
    public object PaymentDetails { get; set; }
}