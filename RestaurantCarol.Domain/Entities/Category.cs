using RestaurantCarol.Domain.Enums;

namespace RestaurantCarol.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public string? ImagePath { get; set; }
}
