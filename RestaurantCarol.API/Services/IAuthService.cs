using RestaurantCarol.Shared.DTOs.Auth;

namespace RestaurantCarol.API.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<UserDto?> GetUserByIdAsync(int userId);
}
