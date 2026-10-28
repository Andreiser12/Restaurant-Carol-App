using RestaurantCarol.Shared.DTOs.Auth;

namespace RestaurantCarol.Client.Services;

public interface IAuthHttpClient
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<UserDto?> GetCurrentUserAsync();
    Task LogoutAsync();
}
