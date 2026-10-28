using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantCarol.API.Services;
using RestaurantCarol.Shared.DTOs.Auth;

namespace RestaurantCarol.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Invalid request payload."
            });
        }

        var response = await _authService.LoginAsync(request);
        if (!response.Success)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Invalid form input."
            });
        }

        var response = await _authService.RegisterAsync(request);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idClaim == null || !int.TryParse(idClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _authService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(user);
    }

    [HttpGet("customer-only")]
    [Authorize(Roles = "Customer")]
    public IActionResult CustomerOnly()
    {
        return Ok(new { message = "Access granted to Customer role!" });
    }

    [HttpGet("chef-only")]
    [Authorize(Roles = "Chef")]
    public IActionResult ChefOnly()
    {
        return Ok(new { message = "Access granted to Chef role!" });
    }

    [HttpGet("delivery-only")]
    [Authorize(Roles = "DeliveryDriver")]
    public IActionResult DeliveryOnly()
    {
        return Ok(new { message = "Access granted to Delivery Driver role!" });
    }

    [HttpGet("manager-only")]
    [Authorize(Roles = "Manager")]
    public IActionResult ManagerOnly()
    {
        return Ok(new { message = "Access granted to Manager role!" });
    }
}
