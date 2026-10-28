using System.Net.Http.Json;
using RestaurantCarol.Shared.DTOs.Menu;
using RestaurantCarol.Shared.Enums;

namespace RestaurantCarol.Client.Services;

public class MenuHttpClient : IMenuHttpClient
{
    private readonly HttpClient _httpClient;

    public MenuHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(CategoryType? type = null)
    {
        try
        {
            var url = type.HasValue ? $"api/menu/categories?type={type.Value}" : "api/menu/categories";
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>(url) ?? new List<CategoryDto>();
        }
        catch
        {
            return new List<CategoryDto>();
        }
    }

    public async Task<List<DishDto>> GetDishesAsync(int? categoryId = null, string? search = null, List<int>? excludedAllergenIds = null)
    {
        try
        {
            var query = new List<string>();
            if (categoryId.HasValue) query.Add($"categoryId={categoryId.Value}");
            if (!string.IsNullOrWhiteSpace(search)) query.Add($"search={Uri.EscapeDataString(search)}");
            if (excludedAllergenIds != null && excludedAllergenIds.Count > 0)
            {
                query.Add($"excludedAllergens={string.Join(",", excludedAllergenIds)}");
            }

            var url = "api/menu/dishes" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
            return await _httpClient.GetFromJsonAsync<List<DishDto>>(url) ?? new List<DishDto>();
        }
        catch
        {
            return new List<DishDto>();
        }
    }

    public async Task<List<DishDto>> GetPopularDishesAsync(int top = 3)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<DishDto>>($"api/menu/dishes/popular?top={top}") ?? new List<DishDto>();
        }
        catch
        {
            return new List<DishDto>();
        }
    }

    public async Task<DishDto?> GetDishByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DishDto>($"api/menu/dishes/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<AllergenDto>> GetAllergensAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<AllergenDto>>("api/menu/allergens") ?? new List<AllergenDto>();
        }
        catch
        {
            return new List<AllergenDto>();
        }
    }
}
