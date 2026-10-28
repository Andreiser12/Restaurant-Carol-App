using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using RestaurantCarol.Client.Auth;
using RestaurantCarol.Shared.DTOs.Auth;

namespace RestaurantCarol.Client.Services;

public class AuthHttpClient : IAuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthHttpClient(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
                {
                    await _authStateProvider.MarkUserAsAuthenticated(result.Token);
                    return result;
                }
            }

            var errorResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return errorResult ?? new AuthResponse
            {
                Success = false,
                Message = "Authentication failed. Please verify your credentials."
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"Unable to connect to server: {ex.Message}"
            };
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
                {
                    await _authStateProvider.MarkUserAsAuthenticated(result.Token);
                    return result;
                }
            }

            var errorResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return errorResult ?? new AuthResponse
            {
                Success = false,
                Message = "Registration failed."
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"Server connection error: {ex.Message}"
            };
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<UserDto>("api/auth/me");
        }
        catch
        {
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        await _authStateProvider.MarkUserAsLoggedOut();
    }
}
