using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    [HttpPost("process")]
    public IActionResult ProcessPayment([FromBody] PaymentRequest request)
    {
        // Simulate payment processing
        if (request.Amount > 0 && !string.IsNullOrEmpty(request.CardNumber))
        {
            var transactionId = Guid.NewGuid().ToString();
            return Ok(new 
            { 
                success = true, 
                transactionId, 
                amount = request.Amount,
                message = "Payment processed successfully" 
            });
        }
        
        return BadRequest(new { success = false, message = "Invalid payment details" });
    }
}

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string CVV { get; set; }
    public int UserId { get; set; }
}