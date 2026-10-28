using RestaurantCarol.Domain.Entities;

namespace RestaurantCarol.Domain.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
