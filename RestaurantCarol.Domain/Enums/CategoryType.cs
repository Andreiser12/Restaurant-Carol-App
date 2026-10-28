namespace RestaurantCarol.Domain.Enums;

public enum CategoryType
{
    Food,
    Drinks
}

public static class CategoryTypeExtensions
{
    public static string ToDbString(this CategoryType type) => type switch
    {
        CategoryType.Food => "Mancare",
        CategoryType.Drinks => "Bauturi",
        _ => "Mancare"
    };

    public static CategoryType FromDbString(string? dbString) => dbString?.Trim() switch
    {
        "Mancare" => CategoryType.Food,
        "Food" => CategoryType.Food,
        "Bauturi" => CategoryType.Drinks,
        "Bautura" => CategoryType.Drinks,
        "Drinks" => CategoryType.Drinks,
        _ => CategoryType.Food
    };
}
