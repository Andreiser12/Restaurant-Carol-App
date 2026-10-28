using RestaurantCarol.Domain.Entities;
using RestaurantCarol.Domain.Enums;
using RestaurantCarol.Domain.Interfaces;
using RestaurantCarol.Infrastructure.Services;
using RestaurantCarol.Shared.DTOs.Auth;

namespace RestaurantCarol.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Email and password are required."
            };
        }

        var user = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLower());
        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        bool isPasswordValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            Success = true,
            Message = "Authentication successful.",
            Token = token,
            User = MapToDto(user)
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var emailExists = await _userRepository.EmailExistsAsync(request.Email.Trim().ToLower());
        if (emailExists)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An account with this email address already exists."
            };
        }

        var passwordHash = PasswordHasher.HashPassword(request.Password);

        var newUser = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim().ToLower(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            PasswordHash = passwordHash,
            Role = (RestaurantCarol.Domain.Enums.UserRole)request.Role,
            LoyaltyPoints = 0
        };

        var newUserId = await _userRepository.CreateUserAsync(newUser);
        newUser.Id = newUserId;

        var token = _tokenService.GenerateToken(newUser);

        return new AuthResponse
        {
            Success = true,
            Message = "Account registered successfully.",
            Token = token,
            User = MapToDto(newUser)
        };
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user == null ? null : MapToDto(user);
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Role = (RestaurantCarol.Shared.Enums.UserRole)user.Role,
        LoyaltyPoints = user.LoyaltyPoints
    };
}
