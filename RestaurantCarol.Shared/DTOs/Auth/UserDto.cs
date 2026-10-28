using RestaurantCarol.Shared.Enums;

namespace RestaurantCarol.Shared.DTOs.Auth;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public string RoleName => Role.ToString();
    public int LoyaltyPoints { get; set; }
}
