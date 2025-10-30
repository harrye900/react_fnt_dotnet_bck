using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class GatewayController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, string> _serviceUrls = new()
    {
        { "auth", Environment.GetEnvironmentVariable("AUTH_SERVICE_URL") ?? "http://localhost:5001" },
        { "products", Environment.GetEnvironmentVariable("PRODUCT_SERVICE_URL") ?? "http://localhost:5002" },
        { "cart", Environment.GetEnvironmentVariable("CART_SERVICE_URL") ?? "http://localhost:5003" },
        { "payment", Environment.GetEnvironmentVariable("PAYMENT_SERVICE_URL") ?? "http://localhost:5004" },
        { "orders", Environment.GetEnvironmentVariable("ORDER_SERVICE_URL") ?? "http://localhost:5005" }
    };

    public GatewayController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] object loginData)
    {
        var json = JsonSerializer.Serialize(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_serviceUrls["auth"]}/api/auth/login", content);
        var result = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, result);
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var response = await _httpClient.GetAsync($"{_serviceUrls["products"]}/api/products");
        var content = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, content);
    }

    [HttpPost("products/add")]
    public async Task<IActionResult> AddProduct([FromBody] object productData)
    {
        var json = JsonSerializer.Serialize(productData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_serviceUrls["products"]}/api/products/add", content);
        var result = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, result);
    }

    [HttpPost("cart/add")]
    public async Task<IActionResult> AddToCart([FromBody] object cartItem)
    {
        var json = JsonSerializer.Serialize(cartItem);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_serviceUrls["cart"]}/api/cart/add", content);
        var result = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, result);
    }

    [HttpPost("payment/process")]
    public async Task<IActionResult> ProcessPayment([FromBody] object paymentData)
    {
        var json = JsonSerializer.Serialize(paymentData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_serviceUrls["payment"]}/api/payment/process", content);
        var result = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, result);
    }

    [HttpPost("orders/create")]
    public async Task<IActionResult> CreateOrder([FromBody] object orderData)
    {
        var json = JsonSerializer.Serialize(orderData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_serviceUrls["orders"]}/api/orders/create", content);
        var result = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, result);
    }

    [HttpGet("orders/{userId}")]
    public async Task<IActionResult> GetOrders(int userId)
    {
        var response = await _httpClient.GetAsync($"{_serviceUrls["orders"]}/api/orders/{userId}");
        var result = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, result);
    }
}