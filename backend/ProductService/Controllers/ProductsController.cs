using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMongoCollection<Product> _products;

    public ProductsController(IMongoDatabase database)
    {
        _products = database.GetCollection<Product>("products");
        SeedData();
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _products.Find(_ => true).ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(string id)
    {
        var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product != null ? Ok(product) : NotFound();
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        };
        
        await _products.InsertOneAsync(product);
        return Ok(new { message = "Product added successfully", productId = product.Id });
    }

    private async void SeedData()
    {
        var count = await _products.CountDocumentsAsync(_ => true);
        if (count == 0)
        {
            await _products.InsertManyAsync(new[]
            {
                new Product { Name = "Laptop", Price = 999.99m, Stock = 11 },
                new Product { Name = "Phone", Price = 599.99m, Stock = 26 },
                new Product { Name = "Headphones", Price = 199.99m, Stock = 50 }
            });
        }
    }
}

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class AddProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}