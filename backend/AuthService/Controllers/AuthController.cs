using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Email == "user@test.com" && request.Password == "password")
        {
            return Ok(new { token = "jwt-token-123", userId = 1, email = request.Email });
        }
        return Unauthorized(new { message = "Invalid credentials" });
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}