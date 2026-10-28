using Microsoft.AspNetCore.Mvc;
using RestaurantCarol.Domain.Entities;
using RestaurantCarol.Domain.Enums;
using RestaurantCarol.Domain.Interfaces;
using RestaurantCarol.Shared.DTOs.Menu;

namespace RestaurantCarol.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuRepository _menuRepository;

    public MenuController(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories([FromQuery] RestaurantCarol.Shared.Enums.CategoryType? type = null)
    {
        var domainType = type.HasValue ? (RestaurantCarol.Domain.Enums.CategoryType?)type.Value : null;
        var categories = await _menuRepository.GetCategoriesAsync(domainType);
        var dtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Type = (RestaurantCarol.Shared.Enums.CategoryType)c.Type,
            ImagePath = c.ImagePath
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("dishes")]
    public async Task<ActionResult<List<DishDto>>> GetDishes(
        [FromQuery] int? categoryId = null,
        [FromQuery] string? search = null,
        [FromQuery] string? excludedAllergens = null)
    {
        List<int>? excludedIds = null;
        if (!string.IsNullOrWhiteSpace(excludedAllergens))
        {
            excludedIds = excludedAllergens
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s.Trim(), out var id) ? id : 0)
                .Where(id => id > 0)
                .ToList();
        }

        var dishes = await _menuRepository.GetDishesAsync(categoryId, search, excludedIds);
        var dtos = dishes.Select(MapDishDto).ToList();
        return Ok(dtos);
    }

    [HttpGet("dishes/popular")]
    public async Task<ActionResult<List<DishDto>>> GetPopularDishes([FromQuery] int top = 3)
    {
        var dishes = await _menuRepository.GetPopularDishesAsync(top);
        var dtos = dishes.Select(MapDishDto).ToList();
        return Ok(dtos);
    }

    [HttpGet("dishes/{id:int}")]
    public async Task<ActionResult<DishDto>> GetDishById(int id)
    {
        var dish = await _menuRepository.GetDishByIdAsync(id);
        if (dish == null)
        {
            return NotFound(new { message = $"Dish with id {id} was not found." });
        }
        return Ok(MapDishDto(dish));
    }

    [HttpGet("allergens")]
    public async Task<ActionResult<List<AllergenDto>>> GetAllergens()
    {
        var allergens = await _menuRepository.GetAllergensAsync();
        var dtos = allergens.Select(a => new AllergenDto
        {
            Id = a.Id,
            Name = a.Name
        }).ToList();
        return Ok(dtos);
    }

    private static DishDto MapDishDto(Dish d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Price = d.Price,
        PortionSize = d.PortionSize,
        TotalStock = d.TotalStock,
        Description = d.Description,
        Calories = d.Calories,
        Fats = d.Fats,
        Carbohydrates = d.Carbohydrates,
        Proteins = d.Proteins,
        Salt = d.Salt,
        CategoryId = d.CategoryId,
        ImagePath = d.ImagePath,
        AllergenIds = d.AllergenIds,
        Allergens = d.Allergens.Select(a => new AllergenDto { Id = a.Id, Name = a.Name }).ToList()
    };
}
