using RestaurantCarol.Domain.Entities;

namespace RestaurantCarol.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email);
    Task<int> CreateUserAsync(User user);
}
