using RestaurantCarol.Shared.DTOs.Menu;
using RestaurantCarol.Shared.Enums;

namespace RestaurantCarol.Client.Services;

public interface IMenuHttpClient
{
    Task<List<CategoryDto>> GetCategoriesAsync(CategoryType? type = null);
    Task<List<DishDto>> GetDishesAsync(int? categoryId = null, string? search = null, List<int>? excludedAllergenIds = null);
    Task<List<DishDto>> GetPopularDishesAsync(int top = 3);
    Task<DishDto?> GetDishByIdAsync(int id);
    Task<List<AllergenDto>> GetAllergensAsync();
}
