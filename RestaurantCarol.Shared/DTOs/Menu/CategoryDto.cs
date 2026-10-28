using RestaurantCarol.Shared.Enums;

namespace RestaurantCarol.Shared.DTOs.Menu;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public string? ImagePath { get; set; }
}
