namespace RestaurantCarol.Shared.DTOs.Menu;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int PortionSize { get; set; }
    public int TotalStock { get; set; }
    public string? Description { get; set; }
    public int? Calories { get; set; }
    public decimal? Fats { get; set; }
    public decimal? Carbohydrates { get; set; }
    public decimal? Proteins { get; set; }
    public decimal? Salt { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public List<int> AllergenIds { get; set; } = new();
    public List<AllergenDto> Allergens { get; set; } = new();
}
