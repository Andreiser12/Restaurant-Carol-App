using RestaurantCarol.Domain.Entities;
using RestaurantCarol.Domain.Enums;

namespace RestaurantCarol.Domain.Interfaces;

public interface IMenuRepository
{
    Task<List<Category>> GetCategoriesAsync(CategoryType? type = null);
    Task<List<Dish>> GetDishesAsync(int? categoryId = null, string? search = null, List<int>? excludedAllergenIds = null);
    Task<List<Dish>> GetPopularDishesAsync(int top = 3);
    Task<Dish?> GetDishByIdAsync(int id);
    Task<List<Allergen>> GetAllergensAsync();
}
